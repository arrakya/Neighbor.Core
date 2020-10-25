using MediatR;
using Neighbor.Core.Application.Response.Finance;

namespace Neighbor.Core.Application.Request.Finance
{
    public class MonthlyBalanceRequest : IRequest<MonthlyBalanceResponse>
    {
        public int Year { get; set; }
    }
}
