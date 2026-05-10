using QuickServePOS.DbContextData.Data;
using QuickServePOS.Repositories.IRepositories;
using QuickServePOS.Repositories.IUnitofWork;
using QuickServePOS.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.UnitofWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public ICategoryRepository Categories { get; }
        public IMenuItemRepository MenuItems { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Categories = new CategoryRepository(_context);
            MenuItems = new MenuItemRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
