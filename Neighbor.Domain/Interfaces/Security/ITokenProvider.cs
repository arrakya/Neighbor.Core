using System.Threading.Tasks;

namespace Neighbor.Core.Domain.Interfaces.Security
{
    public interface ITokenProvider
    {
        Task<string> CreateRefreshToken(string name, string password);

        Task<bool> Validate(string token);

        Task<string> CreateAccessToken(string refreshToken);
    }
}
