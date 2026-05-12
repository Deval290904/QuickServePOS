using QuickServePOS.Models.Entities.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.IRepositories
{
    public interface ITableMergeRepository
    {
        Task AddAsync(TableMergeEntity entity);

        Task<List<TableMergeEntity>> GetActiveMergesAsync();

        Task<List<TableMergeEntity>> GetTableMergesAsync(int tableId);
    }
}
