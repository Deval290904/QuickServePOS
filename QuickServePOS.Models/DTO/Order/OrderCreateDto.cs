using QuickServePOS.Models.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.Order
{
    public class OrderCreateDto
    {
        public int? TableId { get; set; }

        public OrderType OrderType { get; set; }

        public string? Notes { get; set; }
    }
}
