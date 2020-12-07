using System.Threading.Tasks;

namespace Neighbor.Core.Domain.Interfaces.Security
{
    public delegate string GetCurrentRefreshTokenDelegate();
    public delegate Task<byte[]> GetCertificateDelegate();

    public interface IClientTokenProvider : ITokenProvider
    {
        GetCurrentRefreshTokenDelegate GetCurrentRefreshToken { get; }
        GetCertificateDelegate GetCertificate { get; }
    }
}
