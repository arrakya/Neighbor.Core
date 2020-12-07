﻿using MediatR;
using Neighbor.Core.Application.Requests.Security;
using Neighbor.Core.Application.Responses.Security;
using Neighbor.Core.Domain.Interfaces.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Handlers.Security
{
    public class AccessTokenHandler : IRequestHandler<AccessTokenRequest, AccessTokenResponse>
    {
        private readonly ITokenProvider tokenProvider;

        public AccessTokenHandler(ITokenProvider tokenProvider)
        {
            this.tokenProvider = tokenProvider;
        }

        public async Task<AccessTokenResponse> Handle(AccessTokenRequest request, CancellationToken cancellationToken)
        {
            var accessToken = await tokenProvider.CreateAccessToken(request.RefreshToken);
            var response = new AccessTokenResponse
            {
                AccessToken = accessToken
            };
            return response;
        }
    }
}
