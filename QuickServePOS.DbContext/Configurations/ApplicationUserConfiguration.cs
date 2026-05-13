using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickServePOS.Models.Entities.Auth;

namespace QuickServePOS.DbContextData.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            entity.HasQueryFilter(x => !x.IsDeleted);

            entity.HasOne(x => x.UserProfile)
                .WithOne(x => x.User)
                .HasForeignKey<UserProfileEntity>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
