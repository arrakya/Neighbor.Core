using System;
using System.Linq;
using System.Collections.Generic;

namespace Neighbor.Server.Finance.MonthlyBalance.Services
{
    public class MonthlyAverageIncomeCalculatorService
    {
        public IEnumerable<Neighbor.Domain.Models.Finance.MonthlyBalance> CalculateAndSetAverageIncome(IEnumerable<Neighbor.Domain.Models.Finance.MonthlyBalance> models)
        {
            var orderMonthlyHealthModelCollection = models.OrderBy(p => p.MonthNo).ToArray();

            var total = 0d;
            for (int i = 0; i < models.Count(); i++)
            {                
                total += orderMonthlyHealthModelCollection[i].IncomeAmount;
                orderMonthlyHealthModelCollection[i].AverageIncomeAmount = Math.Round(total / (i + 1), 2);
            }

            return orderMonthlyHealthModelCollection;
        }
    }
}