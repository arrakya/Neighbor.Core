using System;
using System.Linq;
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

            var orderMonthlyHealthModelCollection = monthlyBalanceCollection.OrderBy(p => p.MonthNo).ToArray();

            var total = 0d;
            for (int i = 0; i < monthlyBalanceCollection.Count(); i++)
            {
                total += orderMonthlyHealthModelCollection[i].IncomeAmount;
                orderMonthlyHealthModelCollection[i].AverageIncomeAmount = Math.Round(total / (i + 1), 2);
            }            

            total = 0d;
            for (int i = 0; i < monthlyBalanceCollection.Count(); i++)
            {
                total += orderMonthlyHealthModelCollection[i].IncomeAmount;
                orderMonthlyHealthModelCollection[i].TotalIncomeAmount = total;
            }

            var response = new MonthlyBalanceResponse
            {
                Content = monthlyBalanceCollection
            };

            return response;
        }
    }
}
