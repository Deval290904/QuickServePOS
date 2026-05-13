using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickServePOS.Models.Entities.Order;

namespace QuickServePOS.DbContextData.Configurations
{
    public class OrderConfiguration
       : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> entity)
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.OrderNo)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.OrderType)
                .HasConversion<int>();

            entity.Property(x => x.Status)
                .HasConversion<int>();

            entity.Property(x => x.Subtotal)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.TaxAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.DiscountAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.TotalAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.Notes)
                .HasMaxLength(500);

            // INDEXES

            entity.HasIndex(x => x.OrderNo)
                .IsUnique();

            entity.HasIndex(x => x.TableId);

            entity.HasIndex(x => x.Status);

            entity.HasIndex(x => x.OrderType);

            entity.HasIndex(x => x.CreatedAt);

            // QUERY FILTER

            entity.HasQueryFilter(x => !x.IsDeleted);

            // RELATION

            entity.HasOne(x => x.Table)
                .WithMany()
                .HasForeignKey(x => x.TableId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
