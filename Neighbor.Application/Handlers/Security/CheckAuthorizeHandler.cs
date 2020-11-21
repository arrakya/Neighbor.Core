using MediatR;
using Neighbor.Core.Application.Requests.Security;
using Neighbor.Core.Application.Responses.Security;
using Neighbor.Core.Domain.Interfaces.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Handlers.Security
{
    public class CheckAuthorizeHandler : IRequestHandler<CheckAuthorizeRequest, CheckAuthorizeResponse>
    {
        private readonly ITokenProvider tokenProvider;

        public CheckAuthorizeHandler(ITokenProvider tokenProvider)
        {
            this.tokenProvider = tokenProvider;
        }

        public async Task<CheckAuthorizeResponse> Handle(CheckAuthorizeRequest request, CancellationToken cancellationToken)
        {
            var isValid = await tokenProvider.Validate(request.Token);
            var response = new CheckAuthorizeResponse { IsValid = isValid };
            
            return response;
        }
    }
}
