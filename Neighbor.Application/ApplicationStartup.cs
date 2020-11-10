using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Neighbor.Core.Application.Behaviors.Finance;
using Neighbor.Core.Application.Requests.Finance;
using Neighbor.Core.Application.Responses.Finance;
using Neighbor.Core.Domain.Interfaces.Finance;
using Neighbor.Core.Domain.Interfaces.Security;
using Server=Neighbor.Core.Infrastructure.Server;
using Shared = Neighbor.Core.Infrastructure.Shared;
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
            services.AddTransient<ITokenProvider, Shared.TokenProvider>();
        }

        public static void ServerConfigureBuilder(IServiceCollection services)
        {
            services.AddTransient<IFinance, Server.FinanceRepository>();
            services.AddTransient<ITokenProvider, Shared.TokenProvider>();
            services.AddTransient<IPipelineBehavior<MonthlyBalanceRequest, MonthlyBalanceResponse>, MonthBlanceBehavior>();
            
        }
    }
}
