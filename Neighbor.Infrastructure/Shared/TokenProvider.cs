using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Neighbor.Core.Infrastructure.Shared
{
    public class TokenProvider
    {
        private readonly ILogger<TokenProvider> logger;
        private readonly IHostEnvironment hostEnvironment;
        protected string key;

        public TokenProvider(IServiceProvider serviceProvider)
        {
            this.key = Environment.GetEnvironmentVariable("NEIGHBOR_IDENTITY_KEY");
            this.logger = (ILogger<TokenProvider>)serviceProvider.GetService(typeof(ILogger<TokenProvider>));
            this.hostEnvironment = (IHostEnvironment)serviceProvider.GetService(typeof(IHostEnvironment));
        }

        public bool Validate(string tokenString)
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

                return isValid;
            }

            logger?.LogInformation($"Token {(isValid ? "valid": "invalid")} - {tokenString}");

            return isValid;
        }
    }
}
