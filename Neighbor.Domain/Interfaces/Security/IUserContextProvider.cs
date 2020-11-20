using Neighbor.Core.Domain.Models.Identity;
using System.Threading.Tasks;

namespace Neighbor.Core.Domain.Interfaces.Security
{
    public interface IUserContextProvider
    {
        Task<IdentityUserContext> GetUserContext(string userName);
    }
}
