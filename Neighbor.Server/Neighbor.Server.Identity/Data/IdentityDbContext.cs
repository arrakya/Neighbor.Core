using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Neighbor.Core.Infrastructure.Server;
using System;

namespace Neighbor.Server.Identity.Data
{
    public class IdentityDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<IdentityUser>, IIdentityDbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var passwordHasher = new PasswordHasher<IdentityUser>();
            var adminUser = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "arrak.ya@outlook.com",
                NormalizedEmail = "arrak.ya@outlook.com".ToUpper().Normalize(),
                UserName = "arrakya",
                NormalizedUserName = "arrakya".ToUpper().Normalize(),
                PhoneNumber = "0917712328"
            };
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "12345678");

            builder.Entity<IdentityUser>().HasData(new[]
            {
                adminUser
            });

            var adminRole = new IdentityRole("Admin")
            {
                Id = Guid.NewGuid().ToString()
            };
            builder.Entity<IdentityRole>().HasData(new[]
            {
                adminRole
            });

            var userRole = new IdentityUserRole<string>
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            };
            builder.Entity<IdentityUserRole<string>>().HasData(new[]
            {
                userRole
            });

            var arrakFirstNameClaim = new IdentityUserClaim<string>
            {
                Id = 1,
                UserId = adminUser.Id,
                ClaimType = "FirstName",
                ClaimValue = "Arrak"
            };
            var arrakLastNameClaim = new IdentityUserClaim<string>
            {
                Id = 2,
                UserId = adminUser.Id,
                ClaimType = "LastName",
                ClaimValue = "Yambubpah"
            };
            var arrakHouseNoClaim = new IdentityUserClaim<string>
            {
                Id = 3,
                UserId = adminUser.Id,
                ClaimType = "HouseNumber",
                ClaimValue = "89/86"
            };
            builder.Entity<IdentityUserClaim<string>>().HasData(new[] {
                arrakFirstNameClaim,
                arrakLastNameClaim,
                arrakHouseNoClaim
            });
        }
    }
}
