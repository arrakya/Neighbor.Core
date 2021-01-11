using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neighbor.Server.Finance.MonthlyBalance.Data;
using Microsoft.EntityFrameworkCore;
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

            var monthlyBalanceCollection = await dbContext.MonthlyBalances.Where(p => p.Year == year).ToListAsync();

            return monthlyBalanceCollection;
        }
    }
}
