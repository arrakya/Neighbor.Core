using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Globalization;

namespace Neighbor.Server.Finance.MonthlyBalance.Data
{
    public class MonthlyBalanceEntityConfig : IEntityTypeConfiguration<Neighbor.Core.Domain.Models.Finance.MonthlyBalance>
    {
        public void Configure(EntityTypeBuilder<Core.Domain.Models.Finance.MonthlyBalance> builder)
        {
            builder.ToTable("MonthlyBalance", "Finance");
            builder.HasKey(p => new { p.Year, p.MonthNo });
            builder.Ignore(p => p.MonthName);
            builder.Ignore(p => p.AverageIncomeAmount);
            builder.Ignore(p => p.TotalIncomeAmount);
            builder.Ignore(p => p.BalanceAmount);
            
            builder.HasData(new[] {            
                new Core.Domain.Models.Finance.MonthlyBalance { Year = 2020, MonthNo = 1, IncomeAmount = 1872340.0d, ExpenseAmount = -188284.90d },
                new Core.Domain.Models.Finance.MonthlyBalance { Year = 2020, MonthNo = 2, IncomeAmount = 251070.0d, ExpenseAmount = -235860.82d },
                new Core.Domain.Models.Finance.MonthlyBalance { Year = 2020, MonthNo = 3, IncomeAmount = 136430.0d, ExpenseAmount = -266707.34d },
                new Core.Domain.Models.Finance.MonthlyBalance { Year = 2020, MonthNo = 4, IncomeAmount = 95760.0d, ExpenseAmount = -217859.65d },
                new Core.Domain.Models.Finance.MonthlyBalance { Year = 2020, MonthNo = 5, IncomeAmount = 42390.0d, ExpenseAmount = -211140.30d },
                new Core.Domain.Models.Finance.MonthlyBalance { Year = 2020, MonthNo = 6, IncomeAmount = 113990.0d, ExpenseAmount = -283869.92d },
                new Core.Domain.Models.Finance.MonthlyBalance { Year = 2020, MonthNo = 7, IncomeAmount = 367770.0d, ExpenseAmount = -269949.78d },
                new Core.Domain.Models.Finance.MonthlyBalance { Year = 2020, MonthNo = 8, IncomeAmount = 123620.0d, ExpenseAmount = -223690.06d },
                new Core.Domain.Models.Finance.MonthlyBalance { Year = 2020, MonthNo = 9, IncomeAmount = 125480.0d, ExpenseAmount = -267824.75d },
                new Core.Domain.Models.Finance.MonthlyBalance { Year = 2020, MonthNo = 10, IncomeAmount = 0.0d, ExpenseAmount = 0.0d },
                new Core.Domain.Models.Finance.MonthlyBalance { Year = 2020, MonthNo = 11, IncomeAmount = 0.0d, ExpenseAmount = 0.0d },
                new Core.Domain.Models.Finance.MonthlyBalance { Year = 2020, MonthNo = 12, IncomeAmount = 0.0d, ExpenseAmount = 0.0d },
            });
        }
    }
}
