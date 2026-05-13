using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickServePOS.Models.Entities.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.DbContextData.Configurations
{
    public class TableMergeConfiguration : IEntityTypeConfiguration<TableMergeEntity>
    {
        public void Configure(EntityTypeBuilder<TableMergeEntity> entity)
        {
            entity.HasIndex(x => x.PrimaryTableId);

            entity.HasIndex(x => x.ChildTableId);

            entity.HasIndex(x => x.IsActive);

            entity.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
