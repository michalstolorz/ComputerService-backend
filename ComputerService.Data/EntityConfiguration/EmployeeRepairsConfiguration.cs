using ComputerService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerService.Data.EntityConfiguration
{
    public class EmployeeRepairsConfiguration : IEntityTypeConfiguration<EmployeeRepair>
    {
        public void Configure(EntityTypeBuilder<EmployeeRepair> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RepairId)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.HasOne(x => x.Repair)
                .WithMany(y => y.EmployeeRepairs)
                .HasForeignKey(x => x.RepairId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany(y => y.EmployeeRepairs)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
