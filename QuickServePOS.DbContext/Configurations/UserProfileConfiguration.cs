using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickServePOS.Models.Entities.Auth;

namespace QuickServePOS.DbContextData.Configurations
{
    public class UserProfileConfiguration: IEntityTypeConfiguration<UserProfileEntity>
    {
        public void Configure(EntityTypeBuilder<UserProfileEntity> entity)
        {
            entity.HasQueryFilter(x => !x.IsDeleted);

            entity.Property(x => x.Gender)
                .HasMaxLength(20);

            entity.Property(x => x.Address)
                .HasMaxLength(500);
        }
    }
}
