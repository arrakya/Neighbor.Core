using Microsoft.EntityFrameworkCore;
using Neighbor.Core.Domain.Interfaces.Finance;
using Neighbor.Server.Finance.MonthlyBalance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbor.Server.Finance
{
    public class FinanceProvider : IFinance
    {
        private readonly IServiceProvider services;

        public FinanceProvider(IServiceProvider serviceProvider)
        {
            this.services = serviceProvider;
        }

        public async Task<IEnumerable<Core.Domain.Models.Finance.MonthlyBalance>> GetMonthlyBalances(int year)
        {
            var dbContext = (MonthlyBalanceDbContext)services.GetService(typeof(MonthlyBalanceDbContext));

            var monthlyBalanceCollection = await dbContext.MonthlyBalances.Where(p => p.Year == year).ToListAsync();

            return monthlyBalanceCollection;
        }
    }
}
