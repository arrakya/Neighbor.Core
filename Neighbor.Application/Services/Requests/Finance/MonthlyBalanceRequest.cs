using MediatR;
using Neighbor.Core.Application.Responses.Finance;

namespace Neighbor.Core.Application.Requests.Finance
{
    public class MonthlyBalanceRequest : IRequest<MonthlyBalanceResponse>
    {
        public int Year { get; set; }
    }
}
