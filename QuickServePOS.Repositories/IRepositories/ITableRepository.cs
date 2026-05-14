using QuickServePOS.Models.Entities.Order;
using QuickServePOS.Models.Entities.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.IRepositories
{
    public interface ITableRepository
    {
        Task<IEnumerable<RestaurantTableEntity>> GetAllAsync();

        Task<RestaurantTableEntity?> GetByIdAsync(int id);

        Task AddAsync(RestaurantTableEntity entity);

        void Update(RestaurantTableEntity entity);

        void Delete(RestaurantTableEntity entity);

        Task<bool> ExistsAsync(string tableNumber,int floorId,int? excludeId = null);

        Task<RestaurantTableEntity?> GetByIdIgnoreQueryFilterAsync(int id);

       

        Task<List<RestaurantTableEntity>> GetDeletedTablesAsync();
    }
}
