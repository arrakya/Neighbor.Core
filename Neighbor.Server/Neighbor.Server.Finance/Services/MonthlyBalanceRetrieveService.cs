using System.Collections.Generic;
using Neighbor.Core.Domain.Models.Finance;

namespace Neighbor.Server.Finance.MonthlyBalance.Services
{
    public class MonthlyBalanceRetrieveService
    {
        public IEnumerable<Neighbor.Core.Domain.Models.Finance.MonthlyBalance> Retrieve(int year) => 
            new [] {
                new Neighbor.Core.Domain.Models.Finance.MonthlyBalance { MonthNo = 1,  Year = year, MonthName = "Jan", IncomeAmount = 100d, ExpenseAmount = 30d },
                new Neighbor.Core.Domain.Models.Finance.MonthlyBalance { MonthNo = 2,  Year = year, MonthName = "Feb", IncomeAmount = 100d, ExpenseAmount = 30d },
                new Neighbor.Core.Domain.Models.Finance.MonthlyBalance { MonthNo = 3,  Year = year, MonthName = "Mar", IncomeAmount = 100d, ExpenseAmount = 30d },
                new Neighbor.Core.Domain.Models.Finance.MonthlyBalance { MonthNo = 4,  Year = year, MonthName = "Apr", IncomeAmount = 100d, ExpenseAmount = 30d },
                new Neighbor.Core.Domain.Models.Finance.MonthlyBalance { MonthNo = 5,  Year = year, MonthName = "May", IncomeAmount = 100d, ExpenseAmount = 30d },
                new Neighbor.Core.Domain.Models.Finance.MonthlyBalance { MonthNo = 6,  Year = year, MonthName = "Jun", IncomeAmount = 100d, ExpenseAmount = 30d },
                new Neighbor.Core.Domain.Models.Finance.MonthlyBalance { MonthNo = 7,  Year = year, MonthName = "Jul", IncomeAmount = 100d, ExpenseAmount = 30d },
                new Neighbor.Core.Domain.Models.Finance.MonthlyBalance { MonthNo = 8,  Year = year, MonthName = "Aug", IncomeAmount = 100d, ExpenseAmount = 30d },
                new Neighbor.Core.Domain.Models.Finance.MonthlyBalance { MonthNo = 9,  Year = year, MonthName = "Sep", IncomeAmount = 100d, ExpenseAmount = 30d },
                new Neighbor.Core.Domain.Models.Finance.MonthlyBalance { MonthNo = 10, Year = year, MonthName = "Oct", IncomeAmount = 100d, ExpenseAmount = 30d },
                new Neighbor.Core.Domain.Models.Finance.MonthlyBalance { MonthNo = 11, Year = year, MonthName = "Nov", IncomeAmount = 100d, ExpenseAmount = 30d },
                new Neighbor.Core.Domain.Models.Finance.MonthlyBalance { MonthNo = 12, Year = year, MonthName = "Dec", IncomeAmount = 100d, ExpenseAmount = 30d }
            };
    }
}