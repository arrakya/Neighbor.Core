using MediatR;
using Neighbor.Core.Application.Requests.Finance;
using Neighbor.Core.Application.Responses.Finance;
using Neighbor.Core.Domain.Interfaces.Finance;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Handlers.Finance
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
