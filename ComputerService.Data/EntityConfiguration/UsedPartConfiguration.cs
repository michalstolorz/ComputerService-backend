using ComputerService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerService.Data.EntityConfiguration
{
    public class UsedPartConfiguration : IEntityTypeConfiguration<UsedPart>
    {
        public void Configure(EntityTypeBuilder<UsedPart> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Quantity)
                .HasColumnType("decimal(3,0)")
                .IsRequired();

            builder.Property(x => x.RepairId)
                .IsRequired();

            builder.Property(x => x.PartId)
                .IsRequired();

            builder.HasOne(x => x.Repair)
                .WithMany(y => y.UsedParts)
                .HasForeignKey(x => x.RepairId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Part)
                .WithMany(y => y.UsedParts)
                .HasForeignKey(x => x.PartId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
