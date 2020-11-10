using Microsoft.IdentityModel.Tokens;
using Neighbor.Core.Domain.Interfaces.Security;
using System;
using System.Text;

namespace Neighbor.Core.Infrastructure.Shared
{
    public class TokenProvider : ITokenProvider
    {
        private readonly string key;

        public TokenProvider()
        {
            this.key = Environment.GetEnvironmentVariable("NEIGHBOR_IDENTITY_KEY");
        }

        public string Create(double tokenLifeTimeInSec)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("NEIGHBOR_IDENTITY_KEY are empty");
            }

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var tokenDesc = new SecurityTokenDescriptor
            {
                IssuedAt = DateTime.Now
            };
            tokenDesc.Expires = tokenDesc.IssuedAt.Value.AddSeconds(tokenLifeTimeInSec);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(key));
            tokenDesc.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = tokenHandler.CreateJwtSecurityToken(tokenDesc);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
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
