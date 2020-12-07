using Neighbor.Core.Domain.Interfaces.Finance;
using Neighbor.Core.Domain.Interfaces.Security;
using Neighbor.Core.Domain.Models.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Client
{
    public class FinanceRepository : IFinance
    {
        private readonly string baseUri = "/neighbor/finance";
        private readonly HttpClient _httpClient;
        private readonly HttpClient _identityHttpClient;
        private readonly IClientTokenProvider tokenProvider;

        public FinanceRepository(IServiceProvider serviceProvider)
        {
            var httpClientFactory = (IHttpClientFactory)serviceProvider.GetService(typeof(IHttpClientFactory));
            tokenProvider = (IClientTokenProvider)serviceProvider.GetService(typeof(IClientTokenProvider));

            _httpClient = httpClientFactory.CreateClient("finance");
            _identityHttpClient = httpClientFactory.CreateClient("identity");
#if DEBUG
            baseUri = string.Empty;
#endif
        }

        public async Task<string> GetAccessToken()
        {
            var refreshToken = tokenProvider.GetCurrentRefreshToken();

            var formContent = new FormUrlEncodedContent(new Dictionary<string,string>
            {
                { "token", refreshToken }
            });

            var request = new HttpRequestMessage(HttpMethod.Post, "/user/access/token")
            {
                Content = formContent
            };
            request.Headers.Add("Authorization", $"Bearer {refreshToken}");

            var response = await _identityHttpClient.SendAsync(request);
            var accessToken = await response.Content.ReadAsStringAsync();

            return accessToken;
        }

        public virtual async Task<IEnumerable<MonthlyBalance>> GetMonthlyBalances(int year)
        {
            var accessToken = await GetAccessToken();
            
            var requestUri = $"{baseUri}/monthlybalance?year={year}";
            var httpClient = _httpClient;

            var response = await httpClient.GetAsync(requestUri);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return Enumerable.Empty<MonthlyBalance>();
            }

            var responseContent = await response.Content.ReadAsStreamAsync();
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var monthlyBalanceCollection = await JsonSerializer.DeserializeAsync<IEnumerable<MonthlyBalance>>(responseContent, jsonSerializerOptions);

            return monthlyBalanceCollection;
        }
    }
}
