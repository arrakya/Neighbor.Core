using MediatR;
using Neighbor.Core.Application.Requests.Security;
using Neighbor.Core.Application.Responses.Security;
using Neighbor.Core.Domain.Interfaces.Security;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Handlers.Security
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
    {
        private readonly IServiceProvider services;

        public RefreshTokenHandler(IServiceProvider serviceProvider)
        {
            services = serviceProvider;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var tokenProvider = (ITokenProvider)services.GetService(typeof(ITokenProvider));
            var tokens = await tokenProvider.CreateToken(request.Username, request.Password);

            var response = new RefreshTokenResponse 
            { 
                Tokens = tokens
            };

            return response;
        }
    }
}
