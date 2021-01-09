using System.Threading.Tasks;

namespace Neighbor.Core.Domain.Interfaces.Security
{
    public interface ITokenAccessor
    {
        string GetCurrentRefreshToken();
        string GetCurrentAccessToken();
        void SetCurrentAccessToken(string token);
        Task<byte[]> GetCertificate();
    }
}
