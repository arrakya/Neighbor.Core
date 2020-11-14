using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neighbor.Core.Application.Requests.Finance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neighbor.Server.Finance.MonthlyBalance.Controllers
{
    [ApiController]
    [Route("/neighbor/finance/[controller]")]
    public class MonthlyBalanceController : ControllerBase
    {
        private readonly ILogger<MonthlyBalanceController> _logger;
        private readonly IMediator _mediator;

        public MonthlyBalanceController(ILogger<MonthlyBalanceController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<Neighbor.Core.Domain.Models.Finance.MonthlyBalance>> Get(int year)
        {
            var request = new MonthlyBalanceRequest { Year = year };
            var response = await _mediator.Send(request);
            var monthlyBalanceCollection = response.Content;
            
            return monthlyBalanceCollection;
        }
    }
}
