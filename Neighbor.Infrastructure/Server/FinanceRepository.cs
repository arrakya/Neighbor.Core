using Neighbor.Core.Domain.Interfaces.Finance;
using Neighbor.Core.Domain.Models.Finance;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Server
{
    public class FinanceRepository : IFinance
    {
        public virtual async Task<IEnumerable<MonthlyBalance>> GetMonthlyBalances(int year)
        {
            return await Task.FromResult(
                new[] {
                    new MonthlyBalance { MonthNo = 1,  Year = year, MonthName = "Jan", IncomeAmount = 100d, ExpenseAmount = 30d },
                    new MonthlyBalance { MonthNo = 2,  Year = year, MonthName = "Feb", IncomeAmount = 100d, ExpenseAmount = 30d },
                    new MonthlyBalance { MonthNo = 3, Year = year, MonthName = "Mar", IncomeAmount = 100d, ExpenseAmount = 30d },
                    new MonthlyBalance { MonthNo = 4, Year = year, MonthName = "Apr", IncomeAmount = 100d, ExpenseAmount = 30d },
                    new MonthlyBalance { MonthNo = 5, Year = year, MonthName = "May", IncomeAmount = 100d, ExpenseAmount = 30d },
                    new MonthlyBalance { MonthNo = 6, Year = year, MonthName = "Jun", IncomeAmount = 100d, ExpenseAmount = 30d },
                    new MonthlyBalance { MonthNo = 7, Year = year, MonthName = "Jul", IncomeAmount = 100d, ExpenseAmount = 30d },
                    new MonthlyBalance { MonthNo = 8, Year = year, MonthName = "Aug", IncomeAmount = 100d, ExpenseAmount = 30d },
                    new MonthlyBalance { MonthNo = 9, Year = year, MonthName = "Sep", IncomeAmount = 100d, ExpenseAmount = 30d },
                    new MonthlyBalance { MonthNo = 10, Year = year, MonthName = "Oct", IncomeAmount = 100d, ExpenseAmount = 30d },
                    new MonthlyBalance { MonthNo = 11, Year = year, MonthName = "Nov", IncomeAmount = 100d, ExpenseAmount = 30d },
                    new MonthlyBalance { MonthNo = 12, Year = year, MonthName = "Dec", IncomeAmount = 100d, ExpenseAmount = 30d }
                }.AsEnumerable());
        }
    }
}
