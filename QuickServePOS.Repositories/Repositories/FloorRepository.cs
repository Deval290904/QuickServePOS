using Microsoft.EntityFrameworkCore;
using QuickServePOS.DbContextData.Data;
using QuickServePOS.Models.Entities.Table;
using QuickServePOS.Repositories.IRepositories;

namespace QuickServePOS.Repositories.Repositories
{
    public class FloorRepository : IFloorRepository
    {
        private readonly AppDbContext _AppDbcontext;

        public FloorRepository(AppDbContext appDbContext)
        {
            _AppDbcontext = appDbContext;
        }

        public async Task<IEnumerable<FloorEntity>> GetAllAsync()
        {
            return await _AppDbcontext.Floors.Include(x => x.Tables).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<FloorEntity?> GetByIdAsync(int id)
        {
            return await _AppDbcontext.Floors.Include(x => x.Tables).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(FloorEntity entity)
        {
            await _AppDbcontext.Floors.AddAsync(entity);
        }

        public void Update(FloorEntity entity)
        {
            _AppDbcontext.Floors.Update(entity);
        }

        public void Delete(FloorEntity entity)
        {
            entity.IsDeleted = true;

            entity.DeletedAt = DateTime.UtcNow;

            _AppDbcontext.Floors.Update(entity);
        }

        public async Task<bool> ExistsAsync(string name,int? excludeId = null)
        {
            return await _AppDbcontext.Floors.AnyAsync(x =>
                x.Name.ToLower() == name.ToLower()
                &&
                (!excludeId.HasValue || x.Id != excludeId));
        }

        public async Task<FloorEntity?> GetByIdIgnoreQueryFilterAsync(int id)
        {
            return await _AppDbcontext.Floors
                .IgnoreQueryFilters()
                .Include(x => x.Tables)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<FloorEntity>> GetDeletedFloorsAsync()
        {
            return await _AppDbcontext.Floors
                .IgnoreQueryFilters()
                .Where(x => x.IsDeleted)
                .OrderByDescending(x => x.DeletedAt)
                .ToListAsync();
        }
    }
}
