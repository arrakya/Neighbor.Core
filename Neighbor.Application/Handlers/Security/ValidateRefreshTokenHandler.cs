using MediatR;
using Neighbor.Core.Application.Requests.Security;
using Neighbor.Core.Application.Responses.Security;
using Neighbor.Core.Domain.Interfaces.Security;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Handlers.Security
{
    public class ValidateRefreshTokenHandler : IRequestHandler<ValidateRefreshTokenRequest, ValidateRefreshTokenResponse>
    {
        private readonly ITokenProvider tokenProvider;

        public ValidateRefreshTokenHandler(IServiceProvider serviceProvider)
        {
            tokenProvider = (ITokenProvider)serviceProvider.GetService(typeof(ITokenProvider));
        }

        public async Task<ValidateRefreshTokenResponse> Handle(ValidateRefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var isValid = await tokenProvider.Validate(request.RefreshToken);
            var response = new ValidateRefreshTokenResponse { IsValid = isValid };

            return response;
        }
    }
}
