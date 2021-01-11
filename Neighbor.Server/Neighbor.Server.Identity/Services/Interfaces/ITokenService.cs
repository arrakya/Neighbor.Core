using System.Threading.Tasks;

namespace Neighbor.Server.Identity.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateRefreshTokenAsync(string userName, string password);
        Task<string> CreateAccessTokenAsync(string refreshToken);
        Task<bool> ValidateAsync(string tokenString);
    }
}
