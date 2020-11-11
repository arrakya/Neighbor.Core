using Neighbor.Core.Domain.Interfaces.Security;
using Neighbor.Core.Infrastructure.Shared;
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

        public ClientTokenProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("default");
        }

        public async Task<string> Create(double tokenLifeTimeInSec)
        {
            var requestUri = $"{baseUri}/user/autorize";
            var httpClient = _httpClient;
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("username","arrak.ya"),
                new KeyValuePair<string,string>("password","vkiydKN6580")
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
