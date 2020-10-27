using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Neighbor.Core.Infrastructure.Server
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddNeighborInfrastructureDbContext<T>(this IServiceCollection services)
            where T : DbContext, IFinanceDbContext
        {
            services.AddTransient<IFinanceDbContext, T>();

            return services;
        }
    }
}