using System.Linq;
using System.Collections.Generic;

namespace Neighbor.Server.Finance.MonthlyBalance.Services
{
    public class MonthlyTotalIncomeCalculatorService
    {
        public IEnumerable<Neighbor.Core.Domain.Models.Finance.MonthlyBalance> CalculateAndSetTotalIncome(IEnumerable<Neighbor.Core.Domain.Models.Finance.MonthlyBalance> models)
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