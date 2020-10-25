using Autofac;
using Neighbor.Domain.Interfaces.Finance;

namespace Neighbor.Application
{
    public static class ApplicationStartup
    {
        public static void ConfigureBuilder(ContainerBuilder builder)
        {
            builder.RegisterType<FinanceRepository>().As<IFinance>();            
        }
    }
}
