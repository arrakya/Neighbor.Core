using MediatR;
using Neighbor.Core.Application.Responses.Identity;

namespace Neighbor.Core.Application.Requests.Identity
{
    public class CreateUserRequest : IRequest<CreateUserResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string HouseNumber { get; set; }
    }
}
