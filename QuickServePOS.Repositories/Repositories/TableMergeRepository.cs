using Microsoft.EntityFrameworkCore;
using QuickServePOS.DbContextData.Data;
using QuickServePOS.Models.Entities.Table;
using QuickServePOS.Repositories.IRepositories;

namespace QuickServePOS.Repositories.Repositories
{
    public class TableMergeRepository : ITableMergeRepository
    {
        private readonly AppDbContext _AppDbcontext;

        public TableMergeRepository(AppDbContext appDbContext)
        {
            _AppDbcontext = appDbContext;
        }

        public async Task AddAsync(TableMergeEntity entity)
        {
            await _AppDbcontext.TableMerges.AddAsync(entity);
        }

        public async Task<List<TableMergeEntity>> GetActiveMergesAsync()
        {
            return await _AppDbcontext.TableMerges
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<List<TableMergeEntity>> GetTableMergesAsync(int tableId)
        {
            return await _AppDbcontext.TableMerges
                .Where(x =>
                    x.PrimaryTableId == tableId
                    ||
                    x.ChildTableId == tableId)
                .ToListAsync();
        }
    }
}
