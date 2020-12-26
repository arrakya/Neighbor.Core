using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Neighbor.Core.Domain.Interfaces.Finance;
using Neighbor.Core.Domain.Models.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbor.Core.Infrastructure.Server
{
    public class FinanceRepository : IFinance
    {
        private readonly IServiceProvider services;

        public FinanceRepository(IServiceProvider services)
        {
            this.services = services;
        }

        public virtual async Task<IEnumerable<MonthlyBalance>> GetMonthlyBalances(int year)
        {
            var dbContext = (IFinanceDbContext)services.GetRequiredService(typeof(IFinanceDbContext));

            var monthlyBalanceCollection = await dbContext.MonthlyBalances.Where(p => p.Year == year).ToListAsync();

            return monthlyBalanceCollection;
        }
    }
}
