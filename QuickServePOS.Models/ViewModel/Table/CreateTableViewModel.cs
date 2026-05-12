using Microsoft.AspNetCore.Mvc.Rendering;
using QuickServePOS.Models.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ViewModel.Table
{
    public class CreateTableViewModel
    {
        public int FloorId { get; set; }

        public string TableNumber { get; set; } = null!;

        public int Capacity { get; set; }

        public TableStatus Status { get; set; } = TableStatus.Available;

        public List<SelectListItem> Floors { get; set; }= new();
    }
}
