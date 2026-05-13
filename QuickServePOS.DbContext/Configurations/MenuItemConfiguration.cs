using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickServePOS.Models.Entities.Menu;

namespace QuickServePOS.DbContextData.Configurations
{
    public class MenuItemConfiguration: IEntityTypeConfiguration<MenuItemEntity>
    {
        public void Configure(EntityTypeBuilder<MenuItemEntity> entity)
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(500);

            entity.Property(x => x.HalfPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.FullPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.FoodType)
                .HasConversion<int>();

            entity.HasIndex(x => x.CategoryId);

            entity.HasIndex(x => x.IsAvailable);

            entity.HasIndex(x => x.Is86d);

            entity.HasIndex(x => x.IsActive);

            entity.HasQueryFilter(x => !x.IsDeleted);

            entity.HasOne(x => x.Category)
                .WithMany(x => x.MenuItems)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
