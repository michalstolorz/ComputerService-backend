using ComputerService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerService.Data.EntityConfiguration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole", "dbo");

            builder.HasKey(x => new { x.UserId, x.RoleId});

            builder.Property(x => x.RoleId)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(y => y.UserRoles)
                .HasForeignKey(y => y.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Role)
                .WithMany(y => y.UserRoles)
                .HasForeignKey(y => y.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
