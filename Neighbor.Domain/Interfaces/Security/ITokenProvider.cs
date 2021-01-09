using Neighbor.Core.Domain.Models.Security;
using System.Threading.Tasks;

namespace Neighbor.Core.Domain.Interfaces.Security
{
    public interface ITokenProvider
    {
        Task<TokensModel> CreateToken(string name, string password);
        Task<TokensModel> CreateToken(string refreshToken);
        Task<bool> Validate(string token);

    }
}
