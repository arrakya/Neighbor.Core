using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Neighbor.Core.Domain.Interfaces.Finance;

namespace Neighbor.Core.Application
{
    public static class ApplicationStartup
    {
        public static void ConfigureBuilder(ContainerBuilder builder)
        {
            builder.RegisterType<Client.FinanceRepository>().As<IFinance>();
        }

        public static void ConfigureBuilder(IServiceCollection services)
        {
            services.AddTransient<IFinance, Server.FinanceRepository>();
        }
    }
}
