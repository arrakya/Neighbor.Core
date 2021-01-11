using MediatR;
using Neighbor.Core.Application.Requests.Finance;
using Neighbor.Core.Application.Responses.Finance;
using Neighbor.Core.Domain.Interfaces.Finance;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Core.Application.Handlers.Finance
{
    public class MonthlyBalanceHandler : IRequestHandler<MonthlyBalanceRequest, MonthlyBalanceResponse>
    {        
        private readonly IServiceProvider services;

        public MonthlyBalanceHandler(IServiceProvider serviceProvider)
        {
            services = serviceProvider;
        }

        public async Task<MonthlyBalanceResponse> Handle(MonthlyBalanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var finance = (IFinance)services.GetService(typeof(IFinance));
                var monthlyBalanceCollection = await finance.GetMonthlyBalances(request.Year);

                var response = new MonthlyBalanceResponse
                {
                    Content = monthlyBalanceCollection
                };

                return response;
            }
            catch (Exception ex)
            {
                var response = new MonthlyBalanceResponse
                {
                    Exception = ex
                };

                return response;
            }
        }
    }
}
