using Neighbor.Domain.Models.Finance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neighbor.Domain.Interfaces.Finance
{
    public interface IFinance
    {
        Task<IEnumerable<MonthlyBalance>> GetMonthlyBalances(int Year);
    }
}
