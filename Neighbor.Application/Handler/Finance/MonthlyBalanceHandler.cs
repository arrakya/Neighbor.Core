using System;
using System.Linq;
using MediatR;
using Neighbor.Core.Application.Request.Finance;
using Neighbor.Core.Application.Response.Finance;
using Neighbor.Core.Domain.Interfaces.Finance;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

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

            var response = new MonthlyBalanceResponse();

            if (ApplicationStartup.RequestChannel == Request.RequestChannel.Client)
            {
                response.Content = orderMonthlyHealthModelCollection;
                return response;
            }

            var total = 0d;
            for (int i = 0; i < monthlyBalanceCollection.Count(); i++)
            {
                orderMonthlyHealthModelCollection[i].MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1);
                total += orderMonthlyHealthModelCollection[i].IncomeAmount;
                orderMonthlyHealthModelCollection[i].AverageIncomeAmount = Math.Round(total / (i + 1), 2);
                orderMonthlyHealthModelCollection[i].BalanceAmount = orderMonthlyHealthModelCollection[i].AverageIncomeAmount + orderMonthlyHealthModelCollection[i].ExpenseAmount;
            }            

            total = 0d;
            for (int i = 0; i < monthlyBalanceCollection.Count(); i++)
            {
                total += orderMonthlyHealthModelCollection[i].IncomeAmount;
                orderMonthlyHealthModelCollection[i].TotalIncomeAmount = total;
            }

            response.Content = orderMonthlyHealthModelCollection;

            return response;
        }
    }
}
