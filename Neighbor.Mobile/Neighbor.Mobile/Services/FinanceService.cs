using Neighbor.Core.Domain.Interfaces.Finance;
using Neighbor.Core.Domain.Models.Finance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Neighbor.Mobile.Services
{
    public class FinanceService : IFinance
    {
        public Task<IEnumerable<MonthlyBalance>> GetMonthlyBalances(int Year)
        {
            throw new NotImplementedException();
        }
    }
}
