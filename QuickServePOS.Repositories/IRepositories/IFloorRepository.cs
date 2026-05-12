using QuickServePOS.Models.Entities.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.IRepositories
{
    public interface IFloorRepository
    {
        Task<IEnumerable<FloorEntity>> GetAllAsync();

        Task<FloorEntity?> GetByIdAsync(int id);

        Task AddAsync(FloorEntity entity);

        void Update(FloorEntity entity);

        void Delete(FloorEntity entity);

        Task<bool> ExistsAsync(string name,int? excludeId = null);

        Task<FloorEntity?> GetByIdIgnoreQueryFilterAsync(int id);

        Task<List<FloorEntity>> GetDeletedFloorsAsync();
    }
}
