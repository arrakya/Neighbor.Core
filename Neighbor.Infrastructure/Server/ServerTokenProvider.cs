﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Neighbor.Core.Domain.Interfaces.Security;
using Neighbor.Core.Domain.Models.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Neighbor.Core.Infrastructure.Server
{
    public class ServerTokenProvider : ITokenProvider
    {
        protected readonly ILogger<ITokenProvider> logger;
        protected readonly IHostEnvironment hostEnvironment;
        protected readonly IServiceProvider serviceProvider;

        public ServerTokenProvider(IServiceProvider serviceProvider)
        {
            this.logger = (ILogger<ITokenProvider>)serviceProvider.GetService(typeof(ILogger<ITokenProvider>));
            this.hostEnvironment = (IHostEnvironment)serviceProvider.GetService(typeof(IHostEnvironment));
            this.serviceProvider = serviceProvider;
        }

        private X509SecurityKey SecurityKey
        {
            get
            {
                var configure = (IConfiguration)serviceProvider.GetService(typeof(IConfiguration));
                var x509CertificateFilePath = configure.GetSection("Security:CertificatePfxPath").Value;
                var x509Certfificate = new X509Certificate2(x509CertificateFilePath, "vkiydKN6580");
                var x509SecurityKey = new X509SecurityKey(x509Certfificate);

                return x509SecurityKey;
            }
        }

        private string ExtractUserNameFromClaims(string tokenString)
        {
            if (string.IsNullOrEmpty(tokenString))
            {
                logger.LogError("tokenString is empty");
                return string.Empty;
            }

            var token = new JwtSecurityToken(tokenString);
            var tokenHandler = new JwtSecurityTokenHandler();
            var userName = token.Claims.SingleOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name" || p.Type == "unique_name")?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                var logger = (ILogger<ServerTokenProvider>)serviceProvider.GetService(typeof(ILogger<ServerTokenProvider>));
                logger.LogError($"UserName in token not found.");
            }

            return userName;
        }        

        public async Task<TokensModel> CreateToken(string name, string password)
        {
            var userContext = (IUserContextProvider)serviceProvider.GetService(typeof(IUserContextProvider));
            var isValidCredential = await userContext.CheckUserCredential(name, password);

            if (!isValidCredential)
            {
                return default;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDesc = new SecurityTokenDescriptor
            {
                IssuedAt = DateTime.Now
            };
            tokenDesc.Claims = new Dictionary<string, object>
            {
                { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", name }
            };

            var configure = (IConfiguration)serviceProvider.GetService(typeof(IConfiguration));
            var tokenLifeTimeInSec = Convert.ToInt32(configure["Token:LifeTimeSecond:RefreshToken"]);
            var tokenLifetime = (DateTime.Now.AddSeconds(tokenLifeTimeInSec) - DateTime.Now).TotalSeconds;
            tokenDesc.Expires = tokenDesc.IssuedAt.Value.AddSeconds(tokenLifetime);

            tokenDesc.SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.RsaSha256Signature);
            var token = tokenHandler.CreateJwtSecurityToken(tokenDesc);

            var tokenString = await Task.FromResult(tokenHandler.WriteToken(token));

            await userContext.UpdateRefreshTokenInStorage(name, tokenString);

            var tokens = new TokensModel { refresh_token = tokenString };

            return tokens;
        }

        public async Task<TokensModel> CreateToken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var userName = ExtractUserNameFromClaims(refreshToken);

            if (string.IsNullOrEmpty(userName))
            {
                return default;
            }

            var tokenDesc = new SecurityTokenDescriptor
            {
                IssuedAt = DateTime.Now
            };
            tokenDesc.Claims = new Dictionary<string, object>
            {
                { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", userName }
            };

            var configure = (IConfiguration)serviceProvider.GetService(typeof(IConfiguration));
            var tokenLifeTimeInSec = Convert.ToInt32(configure["Token:LifeTimeSecond:AccessToken"]);
            var tokenLifetime = (DateTime.Now.AddSeconds(tokenLifeTimeInSec) - DateTime.Now).TotalSeconds;
            tokenDesc.Expires = tokenDesc.IssuedAt.Value.AddSeconds(tokenLifetime);

            tokenDesc.SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.RsaSha256Signature);
            var newToken = tokenHandler.CreateJwtSecurityToken(tokenDesc);

            var tokenString = await Task.FromResult(tokenHandler.WriteToken(newToken));

            var tokens = new TokensModel { refresh_token = refreshToken, access_token = tokenString };

            return tokens;
        }

        public async Task<bool> Validate(string tokenString)
        {
            var isValid = true;

            var configure = (IConfiguration)serviceProvider.GetService(typeof(IConfiguration));
            var x509CertificateFilePath = configure.GetSection("Security:CertificatePath").Value;

            var x509Certfificate = new X509Certificate2(x509CertificateFilePath);
            var x509SecurityKey = new X509SecurityKey(x509Certfificate);

            var validateParams = new TokenValidationParameters()
            {
                IssuerSigningKey = x509SecurityKey,
                ValidateLifetime = true,
                LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                {
                    var isValidLifeTime = expires > DateTime.UtcNow;

                    return isValidLifeTime;
                },
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(tokenString, validateParams, out var securityToken);
            }
            catch (Exception ex)
            {
                isValid = false;
            }

            return await Task.FromResult(isValid);
        }
    }
}
