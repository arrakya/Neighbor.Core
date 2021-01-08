using Microsoft.AspNetCore.Identity;
using Neighbor.Core.Domain.Interfaces.Security;
using Neighbor.Core.Domain.Models.Identity;
using System;
using System.Linq;
using System.Security.Claims;
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

        public async Task<IdentityUserContext> GetUserContextAsync(string userName)
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
                userContext.FirstName = claims.SingleOrDefault(p => p.Type == "FirstName")?.Value;
                userContext.LastName = claims.SingleOrDefault(p => p.Type == "LastName")?.Value;
                userContext.HouseNumber = claims.SingleOrDefault(p => p.Type == "HouseNumber")?.Value;
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

        public async Task<CreateUserResult> CreateUserAsync(string username, string password, string email, string phone, string houseNumber)
        {
            var passwordHasher = new PasswordHasher<IdentityUser>();            
            var identityUser = new IdentityUser 
            {
                UserName = username,
                Email = email,
                PhoneNumber = phone,
                EmailConfirmed = false,
                NormalizedEmail = email.ToUpper().Normalize(),
                NormalizedUserName = username.ToUpper().Normalize(),
                PhoneNumberConfirmed = false,                
            };
            var hashedPassword = passwordHasher.HashPassword(identityUser, password);
            identityUser.PasswordHash = hashedPassword;

            var result = await userManager.CreateAsync(identityUser);
            var createUserResult = new CreateUserResult();
            if (result.Succeeded)
            {
                var addRoleResult = await userManager.AddToRoleAsync(identityUser, "Member");
                var addClaimResult = await userManager.AddClaimsAsync(identityUser, new[]
                {
                    new Claim("HouseNumber", houseNumber)
                });

                if (addClaimResult.Succeeded)
                {
                    createUserResult.IsSuccess = result.Succeeded;
                    return createUserResult;
                }

                createUserResult.IsSuccess = result.Succeeded;
                createUserResult.ErrorMessage = string.Join("|", addClaimResult.Errors.Select(o => o.Description));
                return createUserResult;
            }

            createUserResult.ErrorMessage = string.Join("|", result.Errors.Select(o => o.Description));
            return createUserResult;
        }
    }
}
