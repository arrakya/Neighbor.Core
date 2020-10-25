using System.Collections.Generic;
using Neighbor.Server.Finance.MonthlyBalance.Models;

namespace Neighbor.Server.Finance.MonthlyBalance.Services
{
    public class MonthlyBalanceRetrieveService
    {
        public IEnumerable<MonthlyBalanceModel> Retrieve(int year) => 
            new [] {
                new MonthlyBalanceModel { MonthNo = 1,  Year = year, MonthName = "Jan", IncomeAmount = 100d, ExpenseAmount = 30d },
                new MonthlyBalanceModel { MonthNo = 2,  Year = year, MonthName = "Feb", IncomeAmount = 100d, ExpenseAmount = 30d },
                new MonthlyBalanceModel { MonthNo = 3,  Year = year, MonthName = "Mar", IncomeAmount = 100d, ExpenseAmount = 30d },
                new MonthlyBalanceModel { MonthNo = 4,  Year = year, MonthName = "Apr", IncomeAmount = 100d, ExpenseAmount = 30d },
                new MonthlyBalanceModel { MonthNo = 5,  Year = year, MonthName = "May", IncomeAmount = 100d, ExpenseAmount = 30d },
                new MonthlyBalanceModel { MonthNo = 6,  Year = year, MonthName = "Jun", IncomeAmount = 100d, ExpenseAmount = 30d },
                new MonthlyBalanceModel { MonthNo = 7,  Year = year, MonthName = "Jul", IncomeAmount = 100d, ExpenseAmount = 30d },
                new MonthlyBalanceModel { MonthNo = 8,  Year = year, MonthName = "Aug", IncomeAmount = 100d, ExpenseAmount = 30d },
                new MonthlyBalanceModel { MonthNo = 9,  Year = year, MonthName = "Sep", IncomeAmount = 100d, ExpenseAmount = 30d },
                new MonthlyBalanceModel { MonthNo = 10, Year = year, MonthName = "Oct", IncomeAmount = 100d, ExpenseAmount = 30d },
                new MonthlyBalanceModel { MonthNo = 11, Year = year, MonthName = "Nov", IncomeAmount = 100d, ExpenseAmount = 30d },
                new MonthlyBalanceModel { MonthNo = 12, Year = year, MonthName = "Dec", IncomeAmount = 100d, ExpenseAmount = 30d }
            };
    }
}