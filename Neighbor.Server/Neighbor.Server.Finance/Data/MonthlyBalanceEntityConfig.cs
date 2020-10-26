using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Neighbor.Server.Finance.MonthlyBalance.Data
{
    public class MonthlyBalanceEntityConfig : IEntityTypeConfiguration<Neighbor.Core.Domain.Models.Finance.MonthlyBalance>
    {
        public void Configure(EntityTypeBuilder<Core.Domain.Models.Finance.MonthlyBalance> builder)
        {
            builder.HasKey(p => new { p.Year, p.MonthNo });
            builder.Ignore(p => p.AverageIncomeAmount);
            builder.Ignore(p => p.TotalIncomeAmount);
            builder.Ignore(p => p.BalanceAmount);
            builder.Property(p => p.MonthName).IsRequired().HasMaxLength(10);
        }
    }
}
