using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neighbor.Core.Application.Requests.Security;
using System.Threading.Tasks;

namespace Neighbor.Server.Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController
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
    }
}
