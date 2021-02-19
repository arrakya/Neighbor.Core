using Microsoft.IdentityModel.Tokens;
using Neighbor.Mobile.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;
using System;

namespace Neighbor.Mobile.Services
{
    public class UserContextService
    {
        public async Task<string> GetUserName()
        {
            var userName = "";
            try
            {
                var refreshToken = Preferences.Get("RefreshToken", "UnKnow");

                var assetProvider = DependencyService.Resolve<IAssetsProvider>();
                var x509CertificateBytes = await assetProvider.Get<byte[]>("arrakya.thddns.net.crt");
                var x509Certfificate = new X509Certificate2(x509CertificateBytes);
                var x509SecurityKey = new X509SecurityKey(x509Certfificate);

                var tokenHandler = new JwtSecurityTokenHandler();
                var cl = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    IssuerSigningKey = x509SecurityKey,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken _);

                userName = cl.Claims.SingleOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
            }
            catch { }

            return userName;
        }
    }
}

