﻿using Neighbor.Core.Domain.Interfaces.Finance;
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
        private readonly ITokenProvider tokenProvider;
        private readonly IClientTokenAccessor tokenAccessor;

        public FinanceRepository(IServiceProvider serviceProvider)
        {
            var httpClientFactory = (IHttpClientFactory)serviceProvider.GetService(typeof(IHttpClientFactory));
            tokenProvider = (ITokenProvider)serviceProvider.GetService(typeof(ITokenProvider));
            tokenAccessor = (IClientTokenAccessor)serviceProvider.GetService(typeof(IClientTokenAccessor));

            _httpClient = httpClientFactory.CreateClient("finance");
        }

        public virtual async Task<IEnumerable<MonthlyBalance>> GetMonthlyBalances(int year)
        {
            var accessToken = tokenAccessor.GetCurrentAccessToken();
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
