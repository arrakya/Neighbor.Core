using MediatR;
using Neighbor.Core.Application.Responses.Security;

namespace Neighbor.Core.Application.Requests.Security
{
    public class AccessTokenRequest : IRequest<AccessTokenResponse>
    {
        public string RefreshToken { get; set; }
    }
}
