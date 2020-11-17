using ComputerService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerService.Data.EntityConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role", "dbo");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasMaxLength(64)
                .IsRequired();

            builder.HasMany(x => x.UserRoles)
                .WithOne(y => y.Role)
                .HasForeignKey(y => y.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
