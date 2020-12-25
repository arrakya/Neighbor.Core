using Neighbor.Core.Domain.Models.Finance;
using System;
using System.Collections.Generic;

namespace Neighbor.Core.Application.Responses.Finance
{
    public class MonthlyBalanceResponse
    {
        public IEnumerable<MonthlyBalance> Content { get; set; }
        public Exception Exception { get; set; }
    }
}
