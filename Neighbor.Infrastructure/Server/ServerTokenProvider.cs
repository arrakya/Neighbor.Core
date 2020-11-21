using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Neighbor.Core.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Neighbor.Core.Infrastructure.Server
{
    public class ServerTokenProvider : ITokenProvider
    {
        protected readonly ILogger<ITokenProvider> logger;
        protected readonly IHostEnvironment hostEnvironment;
        protected readonly IServiceProvider serviceProvider;
        protected string key;

        public ServerTokenProvider(IServiceProvider serviceProvider)
        {
            this.key = Environment.GetEnvironmentVariable("NEIGHBOR_IDENTITY_KEY");
            this.logger = (ILogger<ITokenProvider>)serviceProvider.GetService(typeof(ILogger<ITokenProvider>));
            this.hostEnvironment = (IHostEnvironment)serviceProvider.GetService(typeof(IHostEnvironment));
            this.serviceProvider = serviceProvider;
        }

        public async Task<string> Create(string name, string password)
        {
            var userManager = (UserManager<IdentityUser>)serviceProvider.GetService(typeof(UserManager<IdentityUser>));
            var userIdentity = await userManager.FindByNameAsync(name.ToUpper().Normalize());

            if(userIdentity == null)
            {
                return string.Empty;
            }

            var singInManager = (SignInManager<IdentityUser>)serviceProvider.GetService(typeof(SignInManager<IdentityUser>));
            var signInResult = await singInManager.CheckPasswordSignInAsync(userIdentity, password, false);
            if (!signInResult.Succeeded)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("NEIGHBOR_IDENTITY_KEY are empty");
            }

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var tokenDesc = new SecurityTokenDescriptor
            {
                IssuedAt = DateTime.Now
            };
            tokenDesc.Claims = new Dictionary<string, object>
            {
                { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",name }
            };

            var configure = (IConfiguration)serviceProvider.GetService(typeof(IConfiguration));
            var tokenLifeTimeInSec = Convert.ToInt32(configure["Token:LifeTimeSecond"]);
            var tokenLifetime = (DateTime.Now.AddSeconds(tokenLifeTimeInSec) - DateTime.Now).TotalSeconds;
            tokenDesc.Expires = tokenDesc.IssuedAt.Value.AddSeconds(tokenLifetime);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(key));
            tokenDesc.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = tokenHandler.CreateJwtSecurityToken(tokenDesc);

            var tokenString = await Task.FromResult(tokenHandler.WriteToken(token));

            return tokenString;
        }

        public async Task<bool> Validate(string tokenString)
        {
            if (string.IsNullOrEmpty(key))
            {
                logger?.LogCritical("NEIGHBOR_IDENTITY_KEY are empty");

                if (hostEnvironment.IsDevelopment())
                {
                    throw new Exception("NEIGHBOR_IDENTITY_KEY are empty");
                }
            }

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(tokenString);
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(key));
            var validateParams = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                IssuerSigningKey = securityKey,
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
            var isValid = true;

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(tokenString, validateParams, out var securityToken);
            }
            catch (Exception ex)
            {
                logger?.LogError($"Token {(isValid ? "valid" : "invalid")} - {tokenString} - {ex.Message}");
                isValid = false;

                return await Task.FromResult(isValid);
            }

            logger?.LogInformation($"Token {(isValid ? "valid" : "invalid")} - {tokenString}");

            return await Task.FromResult(isValid);
        }
    }
}
