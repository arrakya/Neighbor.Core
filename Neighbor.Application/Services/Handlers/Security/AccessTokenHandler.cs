using MediatR;
using Neighbor.Core.Application.Requests.Security;
using Neighbor.Core.Application.Responses.Security;
using Neighbor.Core.Domain.Interfaces.Security;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Handlers.Security
{
    public class AccessTokenHandler : IRequestHandler<AccessTokenRequest, AccessTokenResponse>
    {
        private readonly IServiceProvider services;

        public AccessTokenHandler(IServiceProvider serviceProvider)
        {
            services = serviceProvider;
        }

        public async Task<AccessTokenResponse> Handle(AccessTokenRequest request, CancellationToken cancellationToken)
        {
            var tokenProvider = (ITokenProvider)services.GetService(typeof(ITokenProvider));
            var tokens = await tokenProvider.CreateToken(request.RefreshToken);
            var response = new AccessTokenResponse
            {
                Tokens = tokens
            };
            return response;
        }
    }
}
