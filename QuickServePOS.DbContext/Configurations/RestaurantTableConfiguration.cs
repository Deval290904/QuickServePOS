using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickServePOS.Models.Entities.Table;

namespace QuickServePOS.DbContextData.Configurations
{
    public class RestaurantTableConfiguration: IEntityTypeConfiguration<RestaurantTableEntity>
    {
        public void Configure(EntityTypeBuilder<RestaurantTableEntity> entity)
        {
            entity.Property(x => x.TableNumber)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.Status)
                .HasConversion<int>();

            entity.HasIndex(x => x.FloorId);

            entity.HasIndex(x => x.Status);

            entity.HasIndex(x => new
            {
                x.FloorId,
                x.TableNumber
            }).IsUnique();

            entity.HasQueryFilter(x => !x.IsDeleted);

            entity.HasOne(x => x.Floor)
                .WithMany(x => x.Tables)
                .HasForeignKey(x => x.FloorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
