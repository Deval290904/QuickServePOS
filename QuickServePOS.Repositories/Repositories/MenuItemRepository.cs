using Microsoft.EntityFrameworkCore;
using QuickServePOS.DbContextData.Data;
using QuickServePOS.Models.Entities.Menu;
using QuickServePOS.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly AppDbContext _AppDbContext;

        public MenuItemRepository(AppDbContext context)
        {
            _AppDbContext = context;
        }
        public async Task<IEnumerable<MenuItemEntity>> GetAllAsync()
        {
            return await _AppDbContext.MenuItems.Include(x => x.Category).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<MenuItemEntity?> GetByIdAsync(int id)
        {
            return await _AppDbContext.MenuItems.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task AddAsync(MenuItemEntity entity)
        {
            await _AppDbContext.MenuItems.AddAsync(entity);
        }
        public void Update(MenuItemEntity entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _AppDbContext.MenuItems.Update(entity);
        }

        public void Delete(MenuItemEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _AppDbContext.MenuItems.Update(entity);
        }

        public async Task<bool> ExistsAsync(string name,int categoryId,int? excludeId = null)
        {
            return await _AppDbContext.MenuItems.AnyAsync(
                x =>x.Name.ToLower() == name.ToLower()
                &&
                x.CategoryId == categoryId
                &&
                (!excludeId.HasValue || x.Id != excludeId));
        }

        public async Task<MenuItemEntity?>GetByIdIgnoreQueryFilterAsync(int id)
        {
            return await _AppDbContext.MenuItems.IgnoreQueryFilters().Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<MenuItemEntity>>GetDeletedMenuItemsAsync()
        {
            return await _AppDbContext.MenuItems
                .IgnoreQueryFilters()
                .Include(x => x.Category)
                .Where(x => x.IsDeleted)
                .OrderByDescending(x => x.DeletedAt)
                .ToListAsync();
        }

        public async Task<List<MenuItemEntity>>GetByCategoryAsync(int categoryId)
        {
            return await _AppDbContext.MenuItems
                .Where(x =>
                    x.CategoryId == categoryId &&
                    x.IsAvailable &&
                    x.IsActive)
                .ToListAsync();
        }
    }
}
