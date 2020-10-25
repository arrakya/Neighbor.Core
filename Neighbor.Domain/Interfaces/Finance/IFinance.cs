using Neighbor.Core.Domain.Models.Finance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neighbor.Core.Domain.Interfaces.Finance
{
    public interface IFinance
    {
        Task<IEnumerable<MonthlyBalance>> GetMonthlyBalances(int Year);
    }
}
