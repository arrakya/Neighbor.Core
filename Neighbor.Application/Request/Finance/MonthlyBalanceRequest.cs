using MediatR;
using Neighbor.Application.Response.Finance;

namespace Neighbor.Application.Request.Finance
{
    public class MonthlyBalanceRequest : IRequest<MonthlyBalanceResponse>
    {
        public int Year { get; set; }
    }
}
