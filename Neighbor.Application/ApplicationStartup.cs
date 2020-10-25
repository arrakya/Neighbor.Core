using Autofac;
using Neighbor.Core.Domain.Interfaces.Finance;

namespace Neighbor.Core.Application
{
    public static class ApplicationStartup
    {
        public static void ConfigureBuilder(ContainerBuilder builder)
        {
            builder.RegisterType<FinanceRepository>().As<IFinance>();            
        }
    }
}
