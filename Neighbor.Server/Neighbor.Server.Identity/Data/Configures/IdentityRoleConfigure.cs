using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Neighbor.Server.Identity.Data.Configures
{
    public class IdentityRoleConfigure : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.ToTable("Role","identity");
            var adminRole = new IdentityRole("Admin")
            {
                Id = "5dfebc39-5671-4899-bf4f-aad46e906d25",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "901cb174-46ce-4b8c-8ff7-dc2eb7c53d89"
            };
            var memberRole = new IdentityRole("Member")
            {
                Id = "70b01944-2be2-40d0-9f2d-85982b0fca98",
                NormalizedName = "MEMBER",
                ConcurrencyStamp = "0d986914-a2a2-4549-8e9e-1df4864df027"
            };
            builder.HasData(new[]
            {
                adminRole,
                memberRole
            });
        }
    }
}
