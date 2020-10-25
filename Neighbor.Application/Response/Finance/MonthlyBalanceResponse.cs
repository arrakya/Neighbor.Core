using Neighbor.Core.Domain.Models.Finance;
using System.Collections.Generic;

namespace Neighbor.Core.Application.Response.Finance
{
    public class MonthlyBalanceResponse
    {
        public IEnumerable<MonthlyBalance> Content { get; set; }
    }
}
