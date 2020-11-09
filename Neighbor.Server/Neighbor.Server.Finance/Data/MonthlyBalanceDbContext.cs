using Microsoft.EntityFrameworkCore;
using Neighbor.Core.Infrastructure.Server;

namespace Neighbor.Server.Finance.MonthlyBalance.Data
{
    public class MonthlyBalanceDbContext : DbContext, IFinanceDbContext
    {
        public DbSet<Neighbor.Core.Domain.Models.Finance.MonthlyBalance> MonthlyBalances {get; set;}

        public MonthlyBalanceDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MonthlyBalanceEntityConfig());
        }
    }
}
