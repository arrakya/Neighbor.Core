using MediatR;
using Neighbor.Core.Application.Responses.Identity;

namespace Neighbor.Core.Application.Requests.Identity
{
    public class GetUserIdentityRequest : IRequest<GetUserIdentityResponse>
    {
        public string UserName { get; set; }
    }
}
