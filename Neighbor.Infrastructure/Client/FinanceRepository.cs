using Neighbor.Core.Domain.Exceptions;
using Neighbor.Core.Domain.Interfaces.Finance;
using Neighbor.Core.Domain.Interfaces.Security;
using Neighbor.Core.Domain.Models.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        }

        public virtual async Task<IEnumerable<MonthlyBalance>> GetMonthlyBalances(int year)
        {
            var accessToken = tokenProvider.GetCurrentAccessToken();
            var isAccessTokenValid = await tokenProvider.Validate(accessToken);

            if (!isAccessTokenValid)
            {
                var refreshToken = tokenProvider.GetCurrentRefreshToken();
                var isRefreshTokenValid = await tokenProvider.Validate(refreshToken);

                if (!isRefreshTokenValid)
                {
                    throw new RefershTokenInvalidException("Refresh token expired");
                }

                var tokens = await tokenProvider.CreateToken(refreshToken);
                accessToken = tokens.access_token;

                tokenProvider.SetCurrentAccessToken(accessToken);
            }

            var requestUri = $"{baseUri}/monthlybalance?year={year}";
            var httpClient = _httpClient;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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
