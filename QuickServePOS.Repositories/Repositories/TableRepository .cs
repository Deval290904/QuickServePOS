using Microsoft.EntityFrameworkCore;
using QuickServePOS.DbContextData.Data;
using QuickServePOS.Models.Entities.Table;
using QuickServePOS.Repositories.IRepositories;

namespace QuickServePOS.Repositories.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly AppDbContext _AppDbcontext;

        public TableRepository(AppDbContext appDbContext)
        {
            _AppDbcontext = appDbContext;
        }

        public async Task<IEnumerable<RestaurantTableEntity>> GetAllAsync()
        {
            return await _AppDbcontext.RestaurantTables
                .Include(x => x.Floor)
                .OrderBy(x => x.TableNumber)
                .ToListAsync();
        }
        public async Task<RestaurantTableEntity?> GetByIdAsync(int id)
        {
            return await _AppDbcontext.RestaurantTables
                .Include(x => x.Floor)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(RestaurantTableEntity entity)
        {
            await _AppDbcontext.RestaurantTables.AddAsync(entity);
        }

        public void Update(RestaurantTableEntity entity)
        {
            _AppDbcontext.RestaurantTables.Update(entity);
        }

        public void Delete(RestaurantTableEntity entity)
        {
            entity.IsDeleted = true;

            entity.DeletedAt = DateTime.UtcNow;

            _AppDbcontext.RestaurantTables.Update(entity);
        }

        public async Task<bool> ExistsAsync(string tableNumber,int floorId,int? excludeId = null)
        {
            return await _AppDbcontext.RestaurantTables.AnyAsync(x =>
                x.TableNumber.ToLower() == tableNumber.ToLower()
                &&
                x.FloorId == floorId
                &&
                (!excludeId.HasValue || x.Id != excludeId));
        }

        public async Task<RestaurantTableEntity?> GetByIdIgnoreQueryFilterAsync(int id)
        {
            return await _AppDbcontext.RestaurantTables
                .IgnoreQueryFilters()
                .Include(x => x.Floor)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<RestaurantTableEntity>> GetDeletedTablesAsync()
        {
            return await _AppDbcontext.RestaurantTables
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Include(x => x.Floor)
                .Where(x => x.IsDeleted)
                .OrderByDescending(x => x.DeletedAt)
                .ToListAsync();
        }
    }
}
