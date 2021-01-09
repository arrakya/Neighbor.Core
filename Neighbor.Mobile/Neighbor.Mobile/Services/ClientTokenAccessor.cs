using Neighbor.Core.Domain.Interfaces.Security;
using Neighbor.Mobile.Shared;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Neighbor.Mobile.Services
{
    public class ClientTokenAccessor : ITokenAccessor
    {
        public Task<byte[]> GetCertificate()
        {
            var assetProvider = DependencyService.Resolve<IAssetsProvider>();
            var certBytes = assetProvider.Get<byte[]>("arrakya.thddns.net.crt");

            return certBytes;
        }

        public string GetCurrentAccessToken()
        {
            Application.Current.Properties.TryGetValue("access_token", out var accessToken);

            return accessToken?.ToString() ?? string.Empty;
        }

        public string GetCurrentRefreshToken()
        {
            Application.Current.Properties.TryGetValue("refresh_token", out var refreshToken);

            return refreshToken?.ToString() ?? string.Empty;
        }

        public void SetCurrentAccessToken(string token)
        {
            if (Application.Current.Properties.ContainsKey("access_token"))
            {
                Application.Current.Properties.Remove("access_token");
            }

            Application.Current.Properties.Add("access_token", token);
        }
    }
}
