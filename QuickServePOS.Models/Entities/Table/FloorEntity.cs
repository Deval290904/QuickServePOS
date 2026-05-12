using QuickServePOS.Models.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.Entities.Table
{
    public class FloorEntity : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<RestaurantTableEntity> Tables { get; set; }= new List<RestaurantTableEntity>();
    }
}
