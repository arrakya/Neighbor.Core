using Microsoft.IdentityModel.Tokens;
using Neighbor.Core.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Neighbor.Core.Infrastructure.Client
{
    public class ClientTokenProvider : IClientTokenProvider
    {
        private readonly string baseUri = "/neighbor/identity";
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider serviceProvider;

        public ClientTokenProvider(IServiceProvider serviceProvider)
        {
            var httpClientFactory = (IHttpClientFactory)serviceProvider.GetService(typeof(IHttpClientFactory));
            _httpClient = httpClientFactory.CreateClient("identity");

#if DEBUG
            baseUri = string.Empty;
            this.serviceProvider = serviceProvider;
#endif
        }

        public GetCurrentRefreshTokenDelegate GetCurrentRefreshToken { get; set; }

        public GetCertificateDelegate GetCertificate { get; set; }

        public async Task<string> CreateRefreshToken(string name, string password)
        {
            var requestUri = $"{baseUri}/user/authorize";
            var httpClient = _httpClient;
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("username",name),
                new KeyValuePair<string,string>("password",password)
            });
            var response = await httpClient.PostAsync(requestUri, formContent);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return string.Empty;
            }

            var responseToken = await response.Content.ReadAsStringAsync();

            return responseToken;
        }

        public async Task<string> CreateAccessToken(string refreshToken)
        {
            var requestUri = $"{baseUri}/user/access/token";
            var httpClient = _httpClient;
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("token",refreshToken)
            });
            var response = await httpClient.PostAsync(requestUri, formContent);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return string.Empty;
            }

            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        public async Task<bool> Validate(string tokenString)
        {
            var isValid = true;
            var x509CertificateBytes = await GetCertificate();
            var x509Certfificate = new X509Certificate2(x509CertificateBytes);
            var x509SecurityKey = new X509SecurityKey(x509Certfificate);

            var validateParams = new TokenValidationParameters()
            {
                IssuerSigningKey = x509SecurityKey,
                ValidateLifetime = true,
                LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                {
                    var isValidLifeTime = expires > DateTime.UtcNow;

                    return isValidLifeTime;
                },
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
