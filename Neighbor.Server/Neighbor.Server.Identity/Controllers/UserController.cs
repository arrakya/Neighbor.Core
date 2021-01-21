using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neighbor.Core.Domain.Models.Identity;
using Neighbor.Core.Domain.Models.Security;
using Neighbor.Server.Identity.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Neighbor.Server.Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IServiceProvider services;

        public IDictionary<string, string> ErrorCodes = new Dictionary<string, string>(new[]
        {
            new KeyValuePair<string,string>("USER001","User not found.")
        });

        public UserController(IServiceProvider serviceProvider)
        {
            this.services = serviceProvider;
        }

        [Authorize(AuthenticationSchemes = "Basic", Policy = "Basic")]
        [HttpPost]
        [Route("oauth/token")]
        public async Task<IActionResult> AccessToken([FromForm] IFormCollection form)
        {
            if (!form.ContainsKey("grant_type"))
            {
                return new BadRequestResult();
            }

            var tokenService = (ITokenService)services.GetService(typeof(ITokenService));
            var grantType = form["grant_type"].ToString();
            var refreshToken = string.Empty;

            switch (grantType.ToLower())
            {
                case "password":
                    var username = form["username"].ToString();
                    var password = form["password"].ToString();

                    refreshToken = await tokenService.CreateRefreshTokenAsync(username, password);

                    if (string.IsNullOrEmpty(refreshToken))
                    {
                        return new ContentResult { Content = "Invalid Credential", StatusCode = Convert.ToInt16(HttpStatusCode.Unauthorized), ContentType = "text/plain" };
                    }

                    break;
                case "refresh_token":
                    refreshToken = form["refresh_token"].ToString();
                    var isRefreshTokenAlive = await tokenService.ValidateAsync(refreshToken);

                    if (!isRefreshTokenAlive)
                    {
                        return new ContentResult { Content = "Invalid Passed Refresh Token", StatusCode = Convert.ToInt16(HttpStatusCode.Unauthorized), ContentType = "text/plain" };
                    }
                    break;
            }

            var accessToken = await tokenService.CreateAccessTokenAsync(refreshToken);
            var jsonOption = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            };

            return Json(new TokensModel
            {
                refresh_token = refreshToken,
                access_token = accessToken
            }, jsonOption);
        }

        [Authorize(AuthenticationSchemes = "Basic", Policy = "Basic")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] IFormCollection form)
        {
            var userName = form["userName"].ToString();
            var password = form["password"].ToString();
            var email = form["email"].ToString();
            var phone = form["phone"].ToString();
            var houseNumber = form["houseNumber"].ToString();

            var userManager = (UserManager<IdentityUser>)services.GetService(typeof(UserManager<IdentityUser>));
            var existedUser = await userManager.FindByNameAsync(userName);

            if (existedUser != null)
            {
                return new ContentResult
                {
                    StatusCode = Convert.ToInt16(HttpStatusCode.BadRequest),
                    Content = "UserName has existed",
                    ContentType = "text/plain"
                };
            }

            var identityUser = new IdentityUser
            {
                UserName = userName,
                Email = email,
                PhoneNumber = phone,
                NormalizedEmail = email.ToUpper().Normalize(),
                NormalizedUserName = userName.ToUpper().Normalize()
            };

            var passwordHashed = new PasswordHasher<IdentityUser>();
            var hashedPassword = passwordHashed.HashPassword(identityUser, password);
            identityUser.PasswordHash = hashedPassword;

            var createUserResult = await userManager.CreateAsync(identityUser);

            if (!createUserResult.Succeeded)
            {
                return new ContentResult
                {
                    StatusCode = Convert.ToInt16(HttpStatusCode.BadRequest),
                    Content = createUserResult.Errors.FirstOrDefault()?.Description,
                    ContentType = "text/plain"
                };
            }

            var houseNumberClaim = new Claim("HouseNumber", houseNumber);
            var createClaimResult = await userManager.AddClaimAsync(identityUser, houseNumberClaim);

            await userManager.AddToRoleAsync(identityUser, "MEMBER");

            if (!createClaimResult.Succeeded)
            {
                await userManager.DeleteAsync(identityUser);

                return new ContentResult
                {
                    StatusCode = Convert.ToInt16(HttpStatusCode.BadRequest),
                    Content = createClaimResult.Errors.FirstOrDefault()?.Description,
                    ContentType = "text/plain"
                };
            }

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("context")]
        public async Task<IdentityUserContext> GetContext()
        {
            var userName = User.Identity.Name;
            var userManager = (UserManager<IdentityUser>)services.GetService(typeof(UserManager<IdentityUser>));
            var userIdentity = await userManager.FindByNameAsync(userName.ToUpper().Normalize());
            var userContext = default(IdentityUserContext);

            if (userIdentity != null)
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

        [Authorize(AuthenticationSchemes = "Basic", Policy = "Basic")]
        [HttpPost]
        [Route("password/reset")]
        public async Task<ResetPasswordResultModel> ResetPassword([FromForm] IFormCollection form)
        {
            var phoneNumber = form["phoneNumber"].ToString();
            var password = form["password"].ToString();

            var userManager = (UserManager<IdentityUser>)services.GetService(typeof(UserManager<IdentityUser>));
            var existedUser = await userManager.Users.SingleOrDefaultAsync(p => p.PhoneNumber == phoneNumber);

            if (existedUser == null)
            {
                return new ResetPasswordResultModel
                {
                    Code = "USER001",
                    Message = ErrorCodes["USER001"],
                    Result = false
                };
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(existedUser);
            var result = await userManager.ResetPasswordAsync(existedUser, token, password);

            return new ResetPasswordResultModel
            {
                Result = result.Succeeded,
                Code = result.Errors?.FirstOrDefault()?.Code,
                Message = result.Errors?.FirstOrDefault()?.Description
            };
        }
    }
}
