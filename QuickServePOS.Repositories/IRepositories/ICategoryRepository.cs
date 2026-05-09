using QuickServePOS.Models.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryEntity>> GetAllAsync();

        Task<CategoryEntity?> GetByIdAsync(int id);

        Task AddAsync(CategoryEntity category);

        void Update(CategoryEntity category);

        void Delete(CategoryEntity category);
        Task<bool> ExistsAsync(string name, int? excludeId = null);
    }
}
