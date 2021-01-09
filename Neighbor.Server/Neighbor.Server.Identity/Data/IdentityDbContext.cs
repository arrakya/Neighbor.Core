using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

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

            var adminRole = new IdentityRole("Admin")
            {
                Id = Guid.NewGuid().ToString(),
                NormalizedName = "ADMIN"
            };
            var memberRole = new IdentityRole("Member")
            {
                Id = Guid.NewGuid().ToString(),
                NormalizedName = "MEMBER"
            };
            builder.Entity<IdentityRole>().HasData(new[]
            {
                adminRole,
                memberRole
            });
        }
    }
}
