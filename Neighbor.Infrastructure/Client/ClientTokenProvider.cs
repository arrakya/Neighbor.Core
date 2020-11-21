using Neighbor.Core.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Neighbor.Core.Infrastructure.Client
{
    public class ClientTokenProvider : ITokenProvider
    {
        private readonly string baseUri = "/neighbor/identity";
        private readonly HttpClient _httpClient;

        public ClientTokenProvider(IServiceProvider serviceProvider)
        {
            var httpClientFactory = (IHttpClientFactory)serviceProvider.GetService(typeof(IHttpClientFactory));
            _httpClient = httpClientFactory.CreateClient("identity");

#if DEBUG
            baseUri = string.Empty;
#endif
        }

        public async Task<string> Create(string name, string password)
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

        public async Task<bool> Validate(string tokenString)
        {
            var requestUri = $"{baseUri}/user/check/token";
            var httpClient = _httpClient;
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("token",tokenString)
            });
            var response = await httpClient.PostAsync(requestUri, formContent);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var isValid = Convert.ToBoolean(responseString);
            return await Task.FromResult(isValid);
        }
    }
}
