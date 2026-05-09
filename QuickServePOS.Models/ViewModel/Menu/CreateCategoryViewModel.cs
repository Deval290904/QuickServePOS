using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ViewModel.Menu
{
    public class CreateCategoryViewModel
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int DisplayOrder { get; set; } = 1;

        public bool IsActive { get; set; } = true;
    }
}
