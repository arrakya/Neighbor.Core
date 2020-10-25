using Neighbor.Core.Domain.Interfaces.Finance;
using Neighbor.Core.Domain.Models.Finance;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;

namespace Neighbor.Core.Application
{
    public class FinanceRepository : IFinance
    {
        private readonly string baseUri = "http://192.168.1.203/neighbor/finance";

        public virtual async Task<IEnumerable<MonthlyBalance>> GetMonthlyBalances(int year)
        {
            var requestUri = $"{baseUri}/monthlybalance?year={year}";
            var handler = new HttpClientHandler();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var httpClient = new HttpClient(handler);

            var response = await httpClient.SendAsync(request);

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
