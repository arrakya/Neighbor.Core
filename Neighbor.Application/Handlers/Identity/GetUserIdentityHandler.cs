using MediatR;
using Neighbor.Core.Application.Requests.Identity;
using Neighbor.Core.Application.Responses.Identity;
using Neighbor.Core.Domain.Interfaces.Security;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Handlers.Identity
{
    public class GetUserIdentityHandler : IRequestHandler<GetUserIdentityRequest, GetUserIdentityResponse>
    {
        private readonly IServiceProvider services;
        public GetUserIdentityHandler(IServiceProvider services)
        {
            this.services = services;
        }

        public async Task<GetUserIdentityResponse> Handle(GetUserIdentityRequest request, CancellationToken cancellationToken)
        {
            var userContextProvider = (IUserContextProvider)services.GetService(typeof(IUserContextProvider));
            var userContext = await userContextProvider.GetUserContextAsync(request.UserName);

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
