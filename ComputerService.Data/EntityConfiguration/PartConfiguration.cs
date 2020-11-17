using ComputerService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerService.Data.EntityConfiguration
{
    public class PartConfiguration : IEntityTypeConfiguration<Part>
    {
        public void Configure(EntityTypeBuilder<Part> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .HasColumnType("decimal(3,0)")
                .IsRequired();

            builder.Property(x => x.PartBoughtPrice)
                .HasColumnType("decimal(8,2)")
                .IsRequired();
        }
    }
}
