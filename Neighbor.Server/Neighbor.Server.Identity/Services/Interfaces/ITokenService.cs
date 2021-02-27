using Neighbor.Server.Identity.Services.Models;
using System.Threading.Tasks;

namespace Neighbor.Server.Identity.Services.Interfaces
{
    public interface ITokenService
    {
        Task<CreateRefreshTokenResult> CreateRefreshTokenAsync(string userName, string password);
        Task<string> CreateAccessTokenAsync(string refreshToken);
        Task<bool> ValidateAsync(string tokenString);
    }
}
