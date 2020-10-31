using MediatR;
using Neighbor.Core.Application.Request.Finance;
using Neighbor.Core.Application.Response.Finance;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Behaviors.Finance
{
    public class MonthBlanceBehavior : IPipelineBehavior<MonthlyBalanceRequest, MonthlyBalanceResponse>
    {
        public async Task<MonthlyBalanceResponse> Handle(MonthlyBalanceRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<MonthlyBalanceResponse> next)
        {
            var response = await next.Invoke();

            var orderMonthlyHealthModelCollection = response.Content.OrderBy(p => p.MonthNo).ToArray();
            
            for (int i = 0; i < orderMonthlyHealthModelCollection.Count(); i++)
            {
                orderMonthlyHealthModelCollection[i].MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(orderMonthlyHealthModelCollection[i].MonthNo);                
                orderMonthlyHealthModelCollection[i].TotalIncomeAmount += orderMonthlyHealthModelCollection.Take(i + 1).Sum(p => p.IncomeAmount);
                orderMonthlyHealthModelCollection[i].AverageIncomeAmount = orderMonthlyHealthModelCollection.Take(i + 1).Average(p => p.IncomeAmount);                
                orderMonthlyHealthModelCollection[i].BalanceAmount = orderMonthlyHealthModelCollection[i].IncomeAmount + orderMonthlyHealthModelCollection[i].ExpenseAmount;
            }

            response.Content = orderMonthlyHealthModelCollection.OrderByDescending(p => p.MonthNo);
            return response;
        }
    }
}

