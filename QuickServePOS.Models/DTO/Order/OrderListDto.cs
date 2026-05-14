using QuickServePOS.Models.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.Order
{
    public class OrderListDto
    {
        public int Id { get; set; }

        public string OrderNo { get; set; } = null!;

        public string? TableNumber { get; set; }

        public OrderType OrderType { get; set; }

        public OrderStatus Status { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
