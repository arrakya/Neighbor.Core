using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Neighbor.Server.Identity.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Neighbor.Server.Identity
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> logger;
        private readonly IServiceProvider services;

        private X509SecurityKey SecurityKey
        {
            get
            {
                var configure = (IConfiguration)services.GetService(typeof(IConfiguration));
                var x509CertificateFilePath = configure.GetSection("Security:CertificatePfxPath").Value;
                var x509Certfificate = new X509Certificate2(x509CertificateFilePath, "vkiydKN6580");
                var x509SecurityKey = new X509SecurityKey(x509Certfificate);

                return x509SecurityKey;
            }
        }

        public TokenService(IServiceProvider serviceProvider)
        {
            services = serviceProvider;
            logger = (ILogger<TokenService>)services.GetService(typeof(ILogger<TokenService>));
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
                logger.LogError($"UserName in token not found.");
            }

            return userName;
        }

        public async Task<string> CreateRefreshTokenAsync(string username, string password)
        {
            var userManager = (UserManager<IdentityUser>)services.GetService(typeof(UserManager<IdentityUser>));
            var userIdentity = await userManager.FindByNameAsync(username.ToUpper().Normalize());

            if (userIdentity == null)
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
                { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", username }
            };

            var configure = (IConfiguration)services.GetService(typeof(IConfiguration));
            var tokenLifeTimeInSec = Convert.ToInt32(configure["Token:LifeTimeSecond:RefreshToken"]);
            var tokenLifetime = (DateTime.Now.AddSeconds(tokenLifeTimeInSec) - DateTime.Now).TotalSeconds;
            tokenDesc.Expires = tokenDesc.IssuedAt.Value.AddSeconds(tokenLifetime);

            tokenDesc.SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.RsaSha256Signature);
            var token = tokenHandler.CreateJwtSecurityToken(tokenDesc);

            var tokenString = await Task.FromResult(tokenHandler.WriteToken(token));            

            await userManager.RemoveAuthenticationTokenAsync(userIdentity, "neighbor", "refresh_token");
            await userManager.SetAuthenticationTokenAsync(userIdentity, "neighbor", "refresh_token", tokenString);

            return tokenString;
        }

        public async Task<string> CreateAccessTokenAsync(string refreshToken)
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

            var configure = (IConfiguration)services.GetService(typeof(IConfiguration));
            var tokenLifeTimeInSec = Convert.ToInt32(configure["Token:LifeTimeSecond:AccessToken"]);
            var tokenLifetime = (DateTime.Now.AddSeconds(tokenLifeTimeInSec) - DateTime.Now).TotalSeconds;
            tokenDesc.Expires = tokenDesc.IssuedAt.Value.AddSeconds(tokenLifetime);

            tokenDesc.SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.RsaSha256Signature);
            var newToken = tokenHandler.CreateJwtSecurityToken(tokenDesc);

            var tokenString = await Task.FromResult(tokenHandler.WriteToken(newToken));

            return tokenString;
        }

        public async Task<bool> ValidateAsync(string tokenString)
        {
            var isValid = true;

            var configure = (IConfiguration)services.GetService(typeof(IConfiguration));
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
            catch
            {
                isValid = false;
            }

            return await Task.FromResult(isValid);
        } 
    }
}
