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
    public class KOTEntityConfiguration: IEntityTypeConfiguration<KOTEntity>
    {
        public void Configure(EntityTypeBuilder<KOTEntity> builder)
        {
            builder.HasKey(x => x.KOTId);

            builder.Property(x => x.KOTNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.Status)
                   .HasConversion<int>();

            builder.HasOne(x => x.Order)
                   .WithMany(x => x.KOTs)
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RestaurantTable)
                   .WithMany()
                   .HasForeignKey(x => x.RestaurantTableId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
