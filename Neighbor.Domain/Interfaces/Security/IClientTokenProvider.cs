using System.Threading.Tasks;

namespace Neighbor.Core.Domain.Interfaces.Security
{
    public delegate string GetCurrentTokenDelegate();
    public delegate void SetCurrentTokenDelegate(string token);
    public delegate Task<byte[]> GetCertificateDelegate();

    public interface IClientTokenProvider : ITokenProvider
    {
        GetCurrentTokenDelegate GetCurrentRefreshToken { get; }
        GetCurrentTokenDelegate GetCurrentAccessToken { get; }
        SetCurrentTokenDelegate SetCurrentAccessToken { get; }
        GetCertificateDelegate GetCertificate { get; }
    }
}
