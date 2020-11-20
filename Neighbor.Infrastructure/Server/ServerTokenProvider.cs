using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Neighbor.Core.Domain.Interfaces.Security;
using Neighbor.Core.Infrastructure.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Neighbor.Core.Infrastructure.Server
{
    public class ServerTokenProvider : TokenProvider, ITokenProvider
    {
        public ServerTokenProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public async Task<string> Create(double tokenLifeTimeInSec, string name, string password)
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
            tokenDesc.Expires = tokenDesc.IssuedAt.Value.AddSeconds(tokenLifeTimeInSec);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(key));
            tokenDesc.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = tokenHandler.CreateJwtSecurityToken(tokenDesc);

            var tokenString = await Task.FromResult(tokenHandler.WriteToken(token));

            return tokenString;
        }
    }
}
