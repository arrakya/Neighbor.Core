using Autofac;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Neighbor.Core.Application.Behaviors.Finance;
using Neighbor.Core.Application.Request.Finance;
using Neighbor.Core.Application.Response.Finance;
using Neighbor.Core.Domain.Interfaces.Finance;

namespace Neighbor.Core.Application
{
    public static class ApplicationStartup
    {
        public static void ClientConfigureBuilder(ContainerBuilder builder)
        {   
            builder.RegisterType<Client.FinanceRepository>().As<IFinance>();
        }

        public static void ServerConfigureBuilder(IServiceCollection services)
        {
            services.AddTransient<IFinance, Server.FinanceRepository>();
            services.AddTransient<IPipelineBehavior<MonthlyBalanceRequest, MonthlyBalanceResponse>, MonthBlanceBehavior>();
        }
    }
}
