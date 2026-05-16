using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.KOT
{
    public class UpdateKOTItemStatusDto
    {
        public int KOTItemId { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
