using MediatR;
using Neighbor.Core.Application.Requests.Identity;
using Neighbor.Core.Application.Responses.Identity;
using Neighbor.Core.Domain.Interfaces.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Handlers.Identity
{
    public class GetUserIdentityHandler : IRequestHandler<GetUserIdentityRequest, GetUserIdentityResponse>
    {
        private readonly IUserContextProvider userContextProvider;
        public GetUserIdentityHandler(IUserContextProvider userIdentityProvider)
        {
            this.userContextProvider = userIdentityProvider;
        }

        public async Task<GetUserIdentityResponse> Handle(GetUserIdentityRequest request, CancellationToken cancellationToken)
        {
            var userContext = await userContextProvider.GetUserContext(request.UserName);

            if (userContext == null)
            {
                return new GetUserIdentityResponse();
            }

            var response = new GetUserIdentityResponse
            {
                Content = userContext
            };

            return response;
        }
    }
}
