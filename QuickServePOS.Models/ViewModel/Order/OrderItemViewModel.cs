using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ViewModel.Order
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }

        public int MenuItemId { get; set; }

        public string MenuItemName { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public string? SpecialInstruction { get; set; }
    }
}
