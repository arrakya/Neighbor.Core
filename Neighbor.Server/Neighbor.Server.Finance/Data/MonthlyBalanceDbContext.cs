using Microsoft.EntityFrameworkCore;

namespace Neighbor.Server.Finance.MonthlyBalance.Data
{
    public class MonthlyBalanceDbContext : DbContext
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
