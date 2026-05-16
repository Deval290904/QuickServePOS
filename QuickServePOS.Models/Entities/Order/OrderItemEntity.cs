using QuickServePOS.Models.Entities.Common;
using QuickServePOS.Models.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.Entities.Order
{
    public class OrderItemEntity : BaseEntity
    {
        public int OrderId { get; set; }

        public int MenuItemId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; } 

        public string? SpecialInstruction { get; set; }

        // New property to track if KOT has been generated for this item
        public bool IsKOTGenerated{ get; set;} = false;

        public int ConfirmedQuantity{ get; set; } = 0;

        //Navigation Properties
        public virtual OrderEntity Order { get; set; } = null!;

        public virtual MenuItemEntity MenuItem { get; set; } = null!;
    }
}
