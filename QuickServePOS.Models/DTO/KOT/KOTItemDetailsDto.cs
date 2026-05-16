using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.KOT
{
    public class KOTItemDetailsDto
    {
        public int KOTItemId { get; set; }

        public string MenuItemName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? SpecialInstruction { get; set; }
    }
}
