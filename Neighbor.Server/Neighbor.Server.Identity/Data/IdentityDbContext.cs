using Microsoft.EntityFrameworkCore;
using Neighbor.Core.Domain.Models.Finance;
using Neighbor.Core.Infrastructure.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neighbor.Server.Identity.Data
{
    public class IdentityDbContext : DbContext, IIdentityDbContext
    {

        public IdentityDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
