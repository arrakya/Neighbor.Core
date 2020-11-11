using Microsoft.IdentityModel.Tokens;
using Neighbor.Core.Domain.Interfaces.Security;
using System;
using System.Text;

namespace Neighbor.Core.Infrastructure.Shared
{
    public class TokenProvider
    {
        protected readonly string key;

        public TokenProvider()
        {
            this.key = Environment.GetEnvironmentVariable("NEIGHBOR_IDENTITY_KEY");
        }

        public bool Validate(string tokenString)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("NEIGHBOR_IDENTITY_KEY are empty");
            }

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(tokenString);
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(key));
            var validateParams = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                IssuerSigningKey = securityKey,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero
            };
            var isValid = true;

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(tokenString, validateParams, out var securityToken);
            }
            catch
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
