using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Neighbor.Server.Identity.Data.Configures;

namespace Neighbor.Server.Identity.Data
{
    public class IdentityDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<IdentityUser>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new IdentityUserConfigure());
            builder.ApplyConfiguration(new IdentityUserRoleConfigure());
            builder.ApplyConfiguration(new IdentityUserLoginConfigure());
            builder.ApplyConfiguration(new IdentityUserClaimConfigure());
            builder.ApplyConfiguration(new IdentityUserTokenConfigure());
            builder.ApplyConfiguration(new IdentityRoleClaimConfigure());
            builder.ApplyConfiguration(new IdentityRoleConfigure());            
        }
    }
}
