using Microsoft.IdentityModel.Tokens;
using Neighbor.Core.Domain.Models.Security;
using Neighbor.Mobile.NativeHelpers;
using Neighbor.Mobile.Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Neighbor.Mobile.Services.Net
{
    public class HttpClientService
    {
        public HttpClientService()
        {
        }

        public enum ClientType
        {
            Finance, Identity
        }

        public Task<CreateHttpClientResult> CreateBasicHttpClientAsync(ClientType clientTypeName)
        {
            var clientTypeNameText = clientTypeName.ToString().ToLower();

            var httpClientFactory = DependencyService.Resolve<IHttpClientFactory>(DependencyFetchTarget.NewInstance);
            var httpClient = httpClientFactory.CreateClient(clientTypeNameText);
            var clientId = Convert.ToBase64String(Encoding.Default.GetBytes($"neighbor_grooveville:3100601614660"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", clientId);

            var result = new CreateHttpClientResult
            {
                HttpClient = httpClient
            };

            return Task.FromResult(result);
        }

        public async Task<CreateHttpClientResult> CreateOAuthHttpClientAsync(ClientType clientTypeName, CancellationToken cancellationToken)
        {
            var result = new CreateHttpClientResult();
            var clientTypeNameText = clientTypeName.ToString().ToLower();
            var httpClientFactory = DependencyService.Resolve<IHttpClientFactory>(DependencyFetchTarget.NewInstance);
            var httpClient = httpClientFactory.CreateClient(clientTypeNameText);
            var hasRefreshToken = Preferences.ContainsKey("RefreshToken");
            var hasAccessToken = Preferences.ContainsKey("AccessToken");
            var isAccessTokenValid = await ValidateAsync(App.AccessToken);
            var isRefreshTokenValid = await ValidateAsync(App.RefreshToken);

            if (!hasRefreshToken || !isRefreshTokenValid)
            {
                MessagingCenter.Send(this, "RefreshTokenExpired");
                return result;
            }

            if (!cancellationToken.IsCancellationRequested && (!hasAccessToken || !isAccessTokenValid))
            {
                var requestUri = $"user/oauth/token";
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("grant_type","refresh_token"),
                    new KeyValuePair<string,string>("refresh_token", App.RefreshToken)
                });

                var identityHttpClientResult = await CreateBasicHttpClientAsync(ClientType.Identity);
                var identityHttpClient = identityHttpClientResult.HttpClient;
                var response = await identityHttpClient.PostAsync(requestUri, formContent);
                var responseTokens = await response.Content.ReadAsStringAsync();
                var tokens = JsonSerializer.Deserialize<TokensModel>(responseTokens);

                App.AccessToken = tokens.access_token;

                var toastHelper = DependencyService.Resolve<IToastHelper>(DependencyFetchTarget.NewInstance);
                toastHelper.Show("Request new token.");
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.AccessToken);
            result.HttpClient = httpClient;

            return result;
        }

        public async Task<bool> ValidateAsync(string tokenString)
        {
            var isValid = true;
            var assetProvider = DependencyService.Resolve<IAssetsProvider>();
            var x509CertificateBytes = await assetProvider.Get<byte[]>("arrakya.thddns.net.crt");
            var x509Certfificate = new X509Certificate2(x509CertificateBytes);
            var x509SecurityKey = new X509SecurityKey(x509Certfificate);

            var validateParams = new TokenValidationParameters()
            {
                IssuerSigningKey = x509SecurityKey,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(tokenString, validateParams, out var _);
            }
            catch
            {
                isValid = false;
            }

            return await Task.FromResult(isValid);
        }
    }
}
