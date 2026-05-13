using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickServePOS.Models.Entities.Table;

namespace QuickServePOS.DbContextData.Configurations
{
    public class FloorConfiguration: IEntityTypeConfiguration<FloorEntity>
    {
        public void Configure(EntityTypeBuilder<FloorEntity> entity)
        {
            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(x => x.Name);

            entity.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
