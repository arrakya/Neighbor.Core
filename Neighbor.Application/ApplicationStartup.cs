using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Neighbor.Core.Application.Behaviors.Finance;
using Neighbor.Core.Application.Requests.Finance;
using Neighbor.Core.Application.Responses.Finance;
using Neighbor.Core.Domain.Interfaces.Finance;
using Neighbor.Core.Domain.Interfaces.Security;
using Server = Neighbor.Core.Infrastructure.Server;
using System;
using System.Net.Http;

namespace Neighbor.Core.Application
{
    public static class ApplicationStartup
    {
        public static void ClientConfigureBuilder<clientTokenAccessorType>(IServiceCollection services,
            Action<HttpClient> financeHttpClientConfigure,
            Action<HttpClient> identityHttpClientConfigure)
            where clientTokenAccessorType : IClientTokenAccessor, new()
        {
            services.AddMediatR(typeof(ApplicationStartup).Assembly);
            services.AddHttpClient("finance", financeHttpClientConfigure).ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var handler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                        {
                            return true;
                        }
                    };
                    return handler;
                });
            services.AddHttpClient("identity", identityHttpClientConfigure).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                    {
                        return true;
                    }
                };
                return handler;
            });
            services.AddTransient<IFinance, Client.FinanceRepository>();
            services.AddTransient(typeof(IClientTokenAccessor), (_) => new clientTokenAccessorType());
            services.AddTransient<ITokenProvider, Neighbor.Core.Infrastructure.Client.ClientTokenProvider>();
        }

        public static void ServerConfigureBuilder(IServiceCollection services)            
        {
            services.AddTransient<IFinance, Server.FinanceRepository>();
            services.AddTransient<ITokenProvider, Neighbor.Core.Infrastructure.Server.ServerTokenProvider>();
            services.AddTransient<IPipelineBehavior<MonthlyBalanceRequest, MonthlyBalanceResponse>, MonthBlanceBehavior>();
        }

        public static void ServerConfigureBuilder<UserContextProvderType>(IServiceCollection services)
            where UserContextProvderType : IUserContextProvider, new()
        {
            services.AddTransient<IFinance, Server.FinanceRepository>();
            services.AddTransient<ITokenProvider, Neighbor.Core.Infrastructure.Server.ServerTokenProvider>();
            services.AddTransient<IPipelineBehavior<MonthlyBalanceRequest, MonthlyBalanceResponse>, MonthBlanceBehavior>();
            services.AddTransient(typeof(IUserContextProvider), (_) => new UserContextProvderType());
        }
    }
}
