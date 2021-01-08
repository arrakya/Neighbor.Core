using Neighbor.Core.Domain.Models.Identity;
using System.Threading.Tasks;

namespace Neighbor.Core.Domain.Interfaces.Security
{
    public interface IUserContextProvider
    {
        Task<IdentityUserContext> GetUserContextAsync(string userName);
        Task<bool> CheckUserCredential(string username, string password);
        Task UpdateRefreshTokenInStorage(string username, string token);
        Task<CreateUserResult> CreateUserAsync(string username, string password, string email, string phone, string houseNumber);
    }
}
