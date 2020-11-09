using Microsoft.EntityFrameworkCore;
using Neighbor.Core.Domain.Interfaces.Finance;
using Neighbor.Core.Domain.Models.Finance;
using Neighbor.Core.Infrastructure.Server;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Neighbor.Core.Application.Server
{
    public class FinanceRepository : IFinance
    {
        private IFinanceDbContext _dbContext;

        public FinanceRepository(IFinanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<IEnumerable<MonthlyBalance>> GetMonthlyBalances(int year)
        {
            var monthlyBalanceCollection = await _dbContext.MonthlyBalances.Where(p => p.Year == year).ToListAsync();

            return monthlyBalanceCollection;
        }
    }
}
