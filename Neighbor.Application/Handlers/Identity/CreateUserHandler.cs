using MediatR;
using Neighbor.Core.Application.Requests.Identity;
using Neighbor.Core.Application.Responses.Identity;
using Neighbor.Core.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Handlers.Identity
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
    {
        private readonly IServiceProvider serviceProvider;

        public CreateUserHandler(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var response = new CreateUserResponse();
            var userContextProvider = (IUserContextProvider)serviceProvider.GetService(typeof(IUserContextProvider));
            var existUser = await userContextProvider.GetUserContextAsync(request.UserName);

            if(existUser != null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Found this UserName existed in system.";
                return response;
            }

            try
            {
                var result = await userContextProvider.CreateUserAsync(request.UserName, request.Password, request.Email, request.Phone, request.HouseNumber);

                response.IsSuccess = result.IsSuccess;
                response.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
