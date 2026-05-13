using QuickServePOS.Models.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.Order
{
    public class OrderUpdateDto
    {
        public int Id { get; set; }

        public OrderStatus Status { get; set; }

        public string? Notes { get; set; }
    }
}
