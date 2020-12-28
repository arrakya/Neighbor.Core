using MediatR;
using Neighbor.Core.Application.Responses.Security;

namespace Neighbor.Core.Application.Requests.Security
{
    public class ValidateTokenRequest : IRequest<ValidateTokenResponse>
    {
        public string Token { get; set; }
    }
}
