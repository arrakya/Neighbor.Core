using Microsoft.IdentityModel.Tokens;
using Neighbor.Core.Domain.Interfaces.Security;
using Neighbor.Core.Domain.Models.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Neighbor.Core.Infrastructure.Client
{
    public class ClientTokenProvider : ITokenProvider
    {
        private readonly string baseUri = "/neighbor/identity";
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider serviceProvider;

        public ClientTokenProvider(IServiceProvider serviceProvider)
        {
            var httpClientFactory = (IHttpClientFactory)serviceProvider.GetService(typeof(IHttpClientFactory));
            _httpClient = httpClientFactory.CreateClient("identity");
            this.serviceProvider = serviceProvider;
        }

        public async Task<TokensModel> CreateToken(string name, string password)
        {
            var requestUri = $"{baseUri}/user/oauth/token";
            var formContent = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string,string>("grant_type","password"),
                    new KeyValuePair<string,string>("username",name),
                    new KeyValuePair<string,string>("password",password)
                });
            var clientId = Convert.ToBase64String(Encoding.Default.GetBytes($"neighbor_grooveville:3100601614660"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", clientId);

            var response = await _httpClient.PostAsync(requestUri, formContent);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return default;
            }

            var responseTokens = await response.Content.ReadAsStringAsync();
            var tokens = JsonSerializer.Deserialize<TokensModel>(responseTokens);

            return tokens;
        }

        public async Task<TokensModel> CreateToken(string refreshToken)
        {
            var requestUri = $"{baseUri}/user/oauth/token";
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("grant_type","refresh_token"),
                new KeyValuePair<string,string>("refresh_token",refreshToken)
            });
            var clientId = Convert.ToBase64String(Encoding.Default.GetBytes($"neighbor_grooveville:3100601614660"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", clientId);
            var response = await _httpClient.PostAsync(requestUri, formContent);
            var responseTokens = await response.Content.ReadAsStringAsync();
            var tokens = JsonSerializer.Deserialize<TokensModel>(responseTokens);

            return tokens;
        }

        public async Task<bool> Validate(string tokenString)
        {
            var tokenAccessor = (IClientTokenAccessor)serviceProvider.GetService(typeof(IClientTokenAccessor));
            var isValid = true;
            var x509CertificateBytes = await tokenAccessor.GetCertificate();
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
            catch (Exception ex)
            {
                isValid = false;
            }

            return await Task.FromResult(isValid);
        }
    }
}
