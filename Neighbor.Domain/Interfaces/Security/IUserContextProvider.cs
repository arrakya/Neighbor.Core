using Neighbor.Core.Domain.Models.Identity;
using System.Threading.Tasks;

namespace Neighbor.Core.Domain.Interfaces.Security
{
    public interface IUserContextProvider
    {
        Task<IdentityUserContext> GetUserContext(string userName);
        Task<bool> CheckUserCredential(string username, string password);
        Task UpdateRefreshTokenInStorage(string username, string token);
    }
}
