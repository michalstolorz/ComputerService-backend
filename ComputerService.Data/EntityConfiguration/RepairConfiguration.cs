using ComputerService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerService.Data.EntityConfiguration
{
    public class RepairConfiguration : IEntityTypeConfiguration<Repair>
    {
        public void Configure(EntityTypeBuilder<Repair> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RepairCost)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.CreateDateTime)
                .HasColumnType("datetime2(0)")
                .IsRequired();

            builder.Property(x => x.FinishDateTime)
                .HasColumnType("datetime2(0)");

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.CustomerId)
                .IsRequired();

            builder.Property(x => x.InvoiceId);

            builder.Property(x => x.Description)
                .HasMaxLength(500);
        }
    }
}
