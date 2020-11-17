using ComputerService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerService.Data.EntityConfiguration
{
    public class RequiredRepairTypeConfiguration : IEntityTypeConfiguration<RequiredRepairType>
    {
        public void Configure(EntityTypeBuilder<RequiredRepairType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RepairId)
                .IsRequired();

            builder.Property(x => x.RepairTypeId)
                .IsRequired();

            builder.HasOne(x => x.Repair)
                .WithMany(y => y.RequiredRepairTypes)
                .HasForeignKey(x => x.RepairId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RepairType)
                .WithMany(y => y.RequiredRepairTypes)
                .HasForeignKey(x => x.RepairTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
