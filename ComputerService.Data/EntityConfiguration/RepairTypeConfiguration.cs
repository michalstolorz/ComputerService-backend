using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ComputerService.Data.Models;

namespace ComputerService.Data.EntityConfiguration
{
    public class RepairTypeConfiguration : IEntityTypeConfiguration<RepairType>
    {
        public void Configure(EntityTypeBuilder<RepairType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(256)
                .IsRequired();
        }
    }
}
