using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickServePOS.Models.Entities.Order;

namespace QuickServePOS.DbContextData.Configurations
{
    public class OrderItemConfiguration
         : IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> entity)
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.SpecialInstruction)
                .HasMaxLength(500);

            // INDEXES

            entity.HasIndex(x => x.OrderId);

            entity.HasIndex(x => x.MenuItemId);

            // QUERY FILTER

            entity.HasQueryFilter(x => !x.IsDeleted);

            // RELATIONS

            entity.HasOne(x => x.Order)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.MenuItem)
                .WithMany()
                .HasForeignKey(x => x.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
