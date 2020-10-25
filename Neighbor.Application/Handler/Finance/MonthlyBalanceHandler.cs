using MediatR;
using Neighbor.Core.Application.Request.Finance;
using Neighbor.Core.Application.Response.Finance;
using Neighbor.Core.Domain.Interfaces.Finance;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Handler.Finance
{
    public class MonthlyBalanceHandler : IRequestHandler<MonthlyBalanceRequest, MonthlyBalanceResponse>
    {
        private readonly IFinance _finance;

        public MonthlyBalanceHandler(IFinance finance)
        {
            _finance = finance;
        }

        public async Task<MonthlyBalanceResponse> Handle(MonthlyBalanceRequest request, CancellationToken cancellationToken)
        {
            var monthlyBalanceCollection = await _finance.GetMonthlyBalances(request.Year);

            var response = new MonthlyBalanceResponse
            {
                Content = monthlyBalanceCollection
            };

            return response;
        }
    }
}
