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

namespace Neighbor.Mobile.Services
{
    public class FinanceProvider : IFinance
    {
        private readonly string baseUri = "/neighbor/finance";
        private readonly IServiceProvider services;

        public FinanceProvider(IServiceProvider serviceProvider)
        {
            services = serviceProvider;
        }

        public async Task<IEnumerable<MonthlyBalance>> GetMonthlyBalances(int year)
        {
            var httpClientFactory = (IHttpClientFactory)services.GetService(typeof(IHttpClientFactory));
            var httpClient = httpClientFactory.CreateClient("finance");

            var tokenAccessor = (ITokenAccessor)services.GetService(typeof(ITokenAccessor));
            var accessToken = tokenAccessor.GetCurrentAccessToken();
            var requestUri = $"{baseUri}/monthlybalance?year={year}";

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
