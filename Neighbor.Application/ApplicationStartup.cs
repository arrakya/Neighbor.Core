using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Neighbor.Core.Application.Behaviors.Finance;
using Neighbor.Core.Application.Request.Finance;
using Neighbor.Core.Application.Response.Finance;
using Neighbor.Core.Domain.Interfaces.Finance;
using System;
using System.Net.Http;

namespace Neighbor.Core.Application
{
    public static class ApplicationStartup
    {
        public static void ClientConfigureBuilder(IServiceCollection services, Action<HttpClient> httpClientConfigure)
        {
            services.AddMediatR(typeof(ApplicationStartup).Assembly);
            services.AddHttpClient("default", httpClientConfigure);
            services.AddTransient<IFinance, Client.FinanceRepository>();
        }

        public static void ServerConfigureBuilder(IServiceCollection services)
        {
            services.AddTransient<IFinance, Server.FinanceRepository>();
            services.AddTransient<IPipelineBehavior<MonthlyBalanceRequest, MonthlyBalanceResponse>, MonthBlanceBehavior>();
        }
    }
}
