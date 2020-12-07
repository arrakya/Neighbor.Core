using MediatR;
using Neighbor.Core.Application.Requests.Security;
using Neighbor.Core.Application.Responses.Security;
using Neighbor.Core.Domain.Interfaces.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Handlers.Security
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
    {
        private readonly ITokenProvider tokenProvider;

        public RefreshTokenHandler(ITokenProvider tokenProvider)
        {
            this.tokenProvider = tokenProvider;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {            
            var refreshToken = await tokenProvider.CreateRefreshToken(request.Username, request.Password);

            var response = new RefreshTokenResponse 
            { 
                RefreshToken = refreshToken 
            };

            return response;
        }
    }
}
