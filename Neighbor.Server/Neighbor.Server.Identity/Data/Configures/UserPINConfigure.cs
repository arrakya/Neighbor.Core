using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neighbor.Server.Identity.Data.Entity;

namespace Neighbor.Server.Identity.Data.Configures
{
    public class UserPINConfigure : IEntityTypeConfiguration<UserPINEntity>
    {
        public void Configure(EntityTypeBuilder<UserPINEntity> builder)
        {
            builder.ToTable("UserPINs", "identity");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.PIN).IsRequired().HasMaxLength(6);
            builder.Property(p => p.Reference).IsRequired().HasMaxLength(6);
            builder.Property(p => p.UserId).IsRequired();
            builder.Property(p => p.ChannelType).IsRequired().HasMaxLength(20);
            builder.Property(p => p.ChannelAddress).IsRequired().HasMaxLength(50);
            builder.HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
