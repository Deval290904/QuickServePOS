using QuickServePOS.Models.Entities.Common;
using QuickServePOS.Models.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.Entities.Table
{
    public class RestaurantTableEntity : BaseEntity
    {
        public int FloorId { get; set; }

        public string TableNumber { get; set; } = null!;

        public int Capacity { get; set; }

        public TableStatus Status { get; set; } = TableStatus.Available;

        public bool IsActive { get; set; } = true;

        public FloorEntity Floor { get; set; } = null!;
    }
}
