using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Neighbor.Core.Domain.Interfaces.Security;
using Neighbor.Core.Infrastructure.Shared;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Neighbor.Core.Infrastructure.Server
{
    public class ServerTokenProvider : TokenProvider, ITokenProvider
    {
        public ServerTokenProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public async Task<string> Create(double tokenLifeTimeInSec)
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

            var tokenString = await Task.FromResult(tokenHandler.WriteToken(token));

            return tokenString;
        }
    }
}
