using MediatR;
using Neighbor.Core.Application.Responses.Security;

namespace Neighbor.Core.Application.Requests.Security
{
    public class ValidateRefreshTokenRequest : IRequest<ValidateRefreshTokenResponse>
    {
        public string RefreshToken { get; set; }
    }
}
