using QuickServePOS.Models.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.RestaurantTable
{
    public class TableListDto
    {
        public int Id { get; set; }

        public int FloorId { get; set; }

        public string FloorName { get; set; } = null!;

        public string TableNumber { get; set; } = null!;

        public int Capacity { get; set; }

        public TableStatus Status { get; set; }

        public bool IsActive { get; set; }
    }
}
