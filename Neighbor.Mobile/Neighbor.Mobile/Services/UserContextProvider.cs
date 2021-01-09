using Neighbor.Core.Domain.Interfaces.Security;
using Neighbor.Core.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Neighbor.Mobile.Services
{
    public class UserContextProvider : IUserContextProvider
    {
        public Task<bool> CheckUserCredential(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<CreateUserResult> CreateUserAsync(string username, string password, string email, string phone, string houseNumber)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUserContext> GetUserContextAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRefreshTokenInStorage(string username, string token)
        {
            throw new NotImplementedException();
        }
    }
}
