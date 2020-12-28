using MediatR;
using Neighbor.Core.Application.Requests.Security;
using Neighbor.Core.Application.Responses.Security;
using Neighbor.Core.Domain.Interfaces.Security;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Handlers.Security
{
    public class ValidateTokenHandler : IRequestHandler<ValidateTokenRequest, ValidateTokenResponse>
    {
        private readonly ITokenProvider tokenProvider;

        public ValidateTokenHandler(IServiceProvider serviceProvider)
        {
            tokenProvider = (ITokenProvider)serviceProvider.GetService(typeof(ITokenProvider));
        }

        public async Task<ValidateTokenResponse> Handle(ValidateTokenRequest request, CancellationToken cancellationToken)
        {
            var isValid = await tokenProvider.Validate(request.Token);
            var response = new ValidateTokenResponse { IsValid = isValid };

            return response;
        }
    }
}
