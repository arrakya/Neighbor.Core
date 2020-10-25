using System.Linq;
using System.Collections.Generic;
using Neighbor.Server.Finance.MonthlyBalance.Models;

namespace Neighbor.Server.Finance.MonthlyBalance.Services
{
    public class MonthlyTotalIncomeCalculatorService
    {
        public IEnumerable<MonthlyBalanceModel> CalculateAndSetTotalIncome(IEnumerable<MonthlyBalanceModel> models)
        {
            var orderMonthlyHealthModelCollection = models.OrderBy(p => p.MonthNo).ToArray();

            var total = 0d;
            for (int i = 0; i < models.Count(); i++)
            {
                total += orderMonthlyHealthModelCollection[i].IncomeAmount;
                orderMonthlyHealthModelCollection[i].TotalIncomeAmount = total;
            }

            return orderMonthlyHealthModelCollection;
        }
    }
}