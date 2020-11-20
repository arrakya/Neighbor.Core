using Microsoft.AspNetCore.Identity;
using Neighbor.Core.Domain.Interfaces.Security;
using Neighbor.Core.Domain.Models.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbor.Core.Infrastructure.Server
{
    public class UserContextProvider : IUserContextProvider
    {
        private readonly UserManager<IdentityUser> userManager;

        public UserContextProvider(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
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
    }
}
