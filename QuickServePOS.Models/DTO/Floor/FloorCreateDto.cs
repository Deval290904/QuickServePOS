using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.Floor
{
    public class FloorCreateDto
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
