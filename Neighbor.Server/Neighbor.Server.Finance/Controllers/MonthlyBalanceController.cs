using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neighbor.Server.Finance.MonthlyBalance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbor.Server.Finance.MonthlyBalance.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MonthlyBalanceController : ControllerBase
    {
        private readonly IServiceProvider services;

        public MonthlyBalanceController(IServiceProvider serviceProvider)
        {
            this.services = serviceProvider;
        }

        [HttpGet]
        public async Task<IEnumerable<Neighbor.Core.Domain.Models.Finance.MonthlyBalance>> Get(int year)
        {
            var dbContext = (MonthlyBalanceDbContext)services.GetService(typeof(MonthlyBalanceDbContext));

            var orderMonthlyHealthModelCollection = await dbContext.MonthlyBalances.Where(p => p.Year == year).ToListAsync();

            for (int i = 0; i < orderMonthlyHealthModelCollection.Count; i++)
            {
                orderMonthlyHealthModelCollection[i].TotalIncomeAmount += orderMonthlyHealthModelCollection.Take(i + 1).Sum(p => p.IncomeAmount);
                orderMonthlyHealthModelCollection[i].AverageIncomeAmount = orderMonthlyHealthModelCollection.Take(i + 1).Average(p => p.IncomeAmount);
                orderMonthlyHealthModelCollection[i].BalanceAmount = orderMonthlyHealthModelCollection[i].IncomeAmount + orderMonthlyHealthModelCollection[i].ExpenseAmount;
            }

            return orderMonthlyHealthModelCollection.OrderByDescending(p => p.MonthNo);
        }
    }
}
