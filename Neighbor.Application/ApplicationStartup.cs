﻿using MediatR;
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
        public static void ClientConfigureBuilder(IServiceCollection services,
            Action<HttpClient> financeHttpClientConfigure,
            Action<HttpClient> identityHttpClientConfigure,
            Func<IServiceProvider, Neighbor.Core.Infrastructure.Client.ClientTokenProvider> clientTokenFactory)
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
            services.AddTransient<ITokenProvider, Neighbor.Core.Infrastructure.Client.ClientTokenProvider>(clientTokenFactory);
            services.AddTransient<IClientTokenProvider, Neighbor.Core.Infrastructure.Client.ClientTokenProvider>(clientTokenFactory);
        }

        public static void ServerConfigureBuilder(IServiceCollection services)
        {
            services.AddTransient<IFinance, Server.FinanceRepository>();
            services.AddTransient<ITokenProvider, Neighbor.Core.Infrastructure.Server.ServerTokenProvider>();
            services.AddTransient<IPipelineBehavior<MonthlyBalanceRequest, MonthlyBalanceResponse>, MonthBlanceBehavior>();
            services.AddTransient<IUserContextProvider, Server.UserContextProvider>();
        }
    }
}
