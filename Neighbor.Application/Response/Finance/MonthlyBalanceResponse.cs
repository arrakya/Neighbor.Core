using Neighbor.Domain.Models.Finance;
using System.Collections.Generic;

namespace Neighbor.Application.Response.Finance
{
    public class MonthlyBalanceResponse
    {
        public IEnumerable<MonthlyBalance> Content { get; set; }
    }
}
