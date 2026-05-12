using QuickServePOS.Models.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.Entities.Table
{
    public class TableMergeEntity : BaseEntity
    {
        public int PrimaryTableId { get; set; }

        public int ChildTableId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime MergedAt { get; set; } = DateTime.UtcNow;
    }
}
