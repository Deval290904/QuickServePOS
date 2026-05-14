using QuickServePOS.Models.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.IRepositories
{
    public interface IMenuItemRepository
    {
        Task<IEnumerable<MenuItemEntity>> GetAllAsync();

        Task<MenuItemEntity?> GetByIdAsync(int id);

        Task AddAsync(MenuItemEntity entity);

        void Update(MenuItemEntity entity);

        void Delete(MenuItemEntity entity);

        Task<bool> ExistsAsync( string name,int categoryId,int? excludeId = null);

        Task<MenuItemEntity?>GetByIdIgnoreQueryFilterAsync(int id);

        Task<List<MenuItemEntity>> GetDeletedMenuItemsAsync();

        Task<List<MenuItemEntity>>GetByCategoryAsync(int categoryId);
    }
}

