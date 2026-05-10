using QuickServePOS.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.IUnitofWork
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }

        IMenuItemRepository MenuItems { get; }

        Task<int> SaveChangesAsync();
    }
}
