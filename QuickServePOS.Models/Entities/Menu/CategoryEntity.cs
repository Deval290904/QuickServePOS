using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.Entities.Menu
{
    public class CategoryEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<MenuItemEntity> MenuItems { get; set; }
            = new List<MenuItemEntity>();
    }
}
