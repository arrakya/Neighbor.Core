using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Neighbor.Server.Identity.Data.Configures
{
    public class IdentityRoleConfigure : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.ToTable("Role","identity");
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
            builder.HasData(new[]
            {
                adminRole,
                memberRole
            });
        }
    }
}
