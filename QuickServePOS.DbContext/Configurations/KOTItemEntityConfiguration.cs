using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickServePOS.Models.Entities.KOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.DbContextData.Configurations
{
    public class KOTItemEntityConfiguration: IEntityTypeConfiguration<KOTItemEntity>
    {
        public void Configure(EntityTypeBuilder<KOTItemEntity> builder)
        {
            builder.HasKey(x => x.KOTItemId);

            builder.Property(x => x.Status)
                   .HasConversion<int>();

            builder.HasOne(x => x.KOT)
                   .WithMany(x => x.KOTItems)
                   .HasForeignKey(x => x.KOTId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.OrderItem)
                   .WithMany(x => x.KOTItems)
                   .HasForeignKey(x => x.OrderItemId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.MenuItem)
                   .WithMany()
                   .HasForeignKey(x => x.MenuItemId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
