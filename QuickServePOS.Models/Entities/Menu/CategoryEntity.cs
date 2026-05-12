using QuickServePOS.Models.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.Entities.Menu
{
    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<MenuItemEntity> MenuItems { get; set; }
            = new List<MenuItemEntity>();
    }
}
