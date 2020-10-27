using Microsoft.EntityFrameworkCore;
using Neighbor.Core.Domain.Models.Finance;

namespace Neighbor.Core.Infrastructure.Server
{
    public interface IFinanceDbContext
    {
        DbSet<MonthlyBalance> MonthlyBalances { get; }
     }
}
