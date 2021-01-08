using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neighbor.Core.Application.Requests.Identity;
using Neighbor.Core.Application.Requests.Security;
using Neighbor.Core.Domain.Models.Identity;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Neighbor.Server.Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
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

            var grantType = form["grant_type"].ToString();
            var refreshToken = string.Empty;

            switch (grantType.ToLower())
            {
                case "password":
                    var username = form["username"].ToString();
                    var password = form["password"].ToString();

                    var requestRefreshToken = new RefreshTokenRequest
                    {
                        Username = username,
                        Password = password
                    };
                    var responseRefreshTokenResponse = await mediator.Send(requestRefreshToken);
                    refreshToken = responseRefreshTokenResponse.Tokens.refresh_token;
                    break;
                case "refresh_token":
                    refreshToken = form["refresh_token"].ToString();

                    var requestValidateRefreshToken = new ValidateTokenRequest
                    {
                        Token = refreshToken
                    };
                    var validateRefershTokenResponse = await mediator.Send(requestValidateRefreshToken);
                    var isRefreshTokenAlive = validateRefershTokenResponse.IsValid;
                    if (!isRefreshTokenAlive)
                    {
                        return new UnauthorizedResult();
                    }
                    break;
            }

            var requestAccessToken = new AccessTokenRequest
            {
                RefreshToken = refreshToken
            };
            var responseAccessToken = await mediator.Send(requestAccessToken);

            var jsonOption = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            };

            return Json(new
            {
                refresh_token = refreshToken,
                access_token = responseAccessToken.Tokens.access_token
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

            var request = new CreateUserRequest
            {
                UserName = userName,
                Password = password,
                Email = email,
                Phone = phone,
                HouseNumber = houseNumber
            };
            var response = await mediator.Send(request);
            var isSuccess = response.IsSuccess;

            if (isSuccess)
            {
                return Ok();
            }

            return new ContentResult
            {
                StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                Content = response.ErrorMessage,
                ContentType = "text/plain"
            };
        }

        [Authorize]
        [HttpPost]
        [Route("context")]
        public async Task<IdentityUserContext> GetContext()
        {
            var request = new GetUserIdentityRequest
            {
                UserName = User.Identity.Name
            };
            var response = await mediator.Send(request);

            return response.Content;
        }



    }
}
