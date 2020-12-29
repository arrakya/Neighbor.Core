using Microsoft.AspNetCore.Identity;
using Neighbor.Core.Domain.Interfaces.Security;
using Neighbor.Core.Domain.Models.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbor.Server.Identity
{
    public class UserContextProvider : IUserContextProvider
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IServiceProvider services;

        public UserContextProvider(IServiceProvider services)
        {
            this.userManager = (UserManager<IdentityUser>)services.GetService(typeof(UserManager<IdentityUser>));
            this.services = services;
        }

        public async Task<IdentityUserContext> GetUserContext(string userName)
        {
            var userIdentity = await userManager.FindByNameAsync(userName.ToUpper().Normalize());            
            var userContext = default(IdentityUserContext);

            if(userIdentity != null)
            {
                userContext = new IdentityUserContext
                {
                    Email = userIdentity.Email,
                    PhoneNumber = userIdentity.PhoneNumber
                };

                var claims = await userManager.GetClaimsAsync(userIdentity);
                userContext.FirstName = claims.Single(p => p.Type == "FirstName").Value;
                userContext.LastName = claims.Single(p => p.Type == "LastName").Value;
                userContext.HouseNumber = claims.Single(p => p.Type == "HouseNumber").Value;
            }

            return userContext;
        }

        public async Task<bool> CheckUserCredential(string username, string password)
        {
            var userManager = (UserManager<IdentityUser>)services.GetService(typeof(UserManager<IdentityUser>));
            var userIdentity = await userManager.FindByNameAsync(username.ToUpper().Normalize());

            if (userIdentity == null)
            {
                return default;
            }

            var singInManager = (SignInManager<IdentityUser>)services.GetService(typeof(SignInManager<IdentityUser>));
            var signInResult = await singInManager.CheckPasswordSignInAsync(userIdentity, password, false);

            return signInResult.Succeeded;            
        }

        public async Task UpdateRefreshTokenInStorage(string username, string token)
        {
            var userManager = (UserManager<IdentityUser>)services.GetService(typeof(UserManager<IdentityUser>));
            var userIdentity = await userManager.FindByNameAsync(username.ToUpper().Normalize());

            await userManager.RemoveAuthenticationTokenAsync(userIdentity, "neighbor", "refresh_token");
            await userManager.SetAuthenticationTokenAsync(userIdentity, "neighbor", "refresh_token", token);
        }
    }
}
