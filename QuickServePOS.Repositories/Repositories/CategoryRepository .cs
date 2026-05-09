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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryEntity>> GetAllAsync()
        {
            return await _context.Categories.OrderBy(x => x.DisplayOrder).ToListAsync();
        }

        public async Task<CategoryEntity?> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(CategoryEntity categoryEntity)
        {
            await _context.Categories.AddAsync(categoryEntity);
        }

        public void Update(CategoryEntity categoryEntity)
        {
            _context.Categories.Update(categoryEntity);
        }

        public void Delete(CategoryEntity categoryEntity)
        {
            categoryEntity.IsDeleted = true;

            _context.Categories.Update(categoryEntity);
        }

        public async Task<bool> ExistsAsync(string name, int? excludeId = null)
        {
            return await _context.Categories.AnyAsync(x =>
                    x.Name.ToLower() == name.ToLower()
                    && (!excludeId.HasValue || x.Id != excludeId));
        }
    }
}
