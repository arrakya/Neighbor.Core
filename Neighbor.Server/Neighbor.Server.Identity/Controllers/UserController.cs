﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neighbor.Core.Application.Requests.Identity;
using Neighbor.Core.Application.Requests.Security;
using Neighbor.Core.Domain.Models.Identity;
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

        [HttpPost]
        [Route("authorize")]
        public async Task<string> Authorize([FromForm]IFormCollection form)
        {
            var request = new AuthorizeRequest
            {
                Username = form["username"].ToString(),
                Password = form["password"].ToString()
            };
            var response = await mediator.Send(request);
            var token = response.Token;

            return token;
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

        [HttpPost]
        [Route("check/token")]
        public async Task<bool> CheckToken([FromForm] IFormCollection form)
        {
            var request = new CheckAuthorizeRequest
            {
                Token = form["token"].ToString(),
            };
            var response = await mediator.Send(request);

            return response.IsValid;
        }
    }
}
