using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neighbor.Server.Finance.MonthlyBalance.Models;
using Neighbor.Server.Finance.MonthlyBalance.Services;
using System.Collections.Generic;

namespace Neighbor.Server.Finance.MonthlyBalance.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonthlyBalanceController : ControllerBase
    {
        private readonly ILogger<MonthlyBalanceController> _logger;
        private readonly MonthlyAverageIncomeCalculatorService _monthlyAverageIncomeCalculatorService;
        private readonly MonthlyBalanceRetrieveService _monthlyBalanceRetrieveService;
        private readonly MonthlyTotalIncomeCalculatorService _monthlyTotalIncomeCalculatorService;

        public MonthlyBalanceController(ILogger<MonthlyBalanceController> logger,
            MonthlyAverageIncomeCalculatorService monthlyAverageIncomeCalculatorService,
            MonthlyBalanceRetrieveService monthlyBalanceRetrieveService,
            MonthlyTotalIncomeCalculatorService MonthlyTotalIncomeCalculatorService)
        {
            _logger = logger;
            _monthlyAverageIncomeCalculatorService = monthlyAverageIncomeCalculatorService;
            _monthlyBalanceRetrieveService = monthlyBalanceRetrieveService;
            _monthlyTotalIncomeCalculatorService = MonthlyTotalIncomeCalculatorService;
        }

        [HttpGet]
        public IEnumerable<MonthlyBalanceModel> Get(int year)
        {
            var monthlyBalanceCollection = _monthlyBalanceRetrieveService.Retrieve(year);
            _monthlyAverageIncomeCalculatorService.CalculateAndSetAverageIncome(monthlyBalanceCollection);
            _monthlyTotalIncomeCalculatorService.CalculateAndSetTotalIncome(monthlyBalanceCollection);

            return monthlyBalanceCollection;
        }
    }
}
