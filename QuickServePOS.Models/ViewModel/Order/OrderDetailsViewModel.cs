using QuickServePOS.Models.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ViewModel.Order
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }

        public string OrderNo { get; set; } = null!;

        public int? TableId { get; set; }

        public string? TableName { get; set; }

        public OrderType OrderType { get; set; }

        public OrderStatus Status { get; set; }

        public decimal Subtotal { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Notes { get; set; }

        public List<OrderItemViewModel> Items { get; set; }
            = new();

    }
}
