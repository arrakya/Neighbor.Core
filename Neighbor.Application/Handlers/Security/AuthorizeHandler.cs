using MediatR;
using Neighbor.Core.Application.Requests.Security;
using Neighbor.Core.Application.Responses.Security;
using Neighbor.Core.Domain.Interfaces.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Handlers.Security
{
    public class AuthorizeHandler : IRequestHandler<AuthorizeRequest, AuthorizeResponse>
    {
        private readonly ITokenProvider tokenProvider;

        public AuthorizeHandler(ITokenProvider tokenProvider)
        {
            this.tokenProvider = tokenProvider;
        }

        public async Task<AuthorizeResponse> Handle(AuthorizeRequest request, CancellationToken cancellationToken)
        {            
            var token = await tokenProvider.Create(request.Username, request.Password);
            var response = new AuthorizeResponse { Token = token };

            return response;
        }
    }
}
