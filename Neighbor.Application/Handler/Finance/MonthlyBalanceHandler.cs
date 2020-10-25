using MediatR;
using Neighbor.Application.Request.Finance;
using Neighbor.Application.Response.Finance;
using Neighbor.Domain.Interfaces.Finance;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Application.Handler.Finance
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
