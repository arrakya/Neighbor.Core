using Neighbor.Core.Domain.Interfaces.Security;
using Neighbor.Core.Domain.Models.Identity;
using Neighbor.Core.Infrastructure.Shared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Neighbor.Core.Infrastructure.Client
{
    public class ClientTokenProvider : TokenProvider, ITokenProvider
    {
        private readonly string baseUri = "/neighbor/identity";
        private readonly HttpClient _httpClient;

        public ClientTokenProvider(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            var httpClientFactory = (IHttpClientFactory)serviceProvider.GetService(typeof(IHttpClientFactory));
            _httpClient = httpClientFactory.CreateClient("identity");

            key = "310060161466031006016146603100601614660";
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
    }
}
