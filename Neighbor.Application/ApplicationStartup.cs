using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Neighbor.Core.Application.Request;
using Neighbor.Core.Domain.Interfaces.Finance;

namespace Neighbor.Core.Application
{
    public static class ApplicationStartup
    {
        public static RequestChannel RequestChannel;

        public static void ConfigureBuilder(ContainerBuilder builder)
        {
            builder.RegisterType<Client.FinanceRepository>().As<IFinance>();
            RequestChannel = RequestChannel.Client;
        }

        public static void ConfigureBuilder(IServiceCollection services)
        {
            services.AddTransient<IFinance, Server.FinanceRepository>();
            RequestChannel = RequestChannel.Server;
        }
    }
}
