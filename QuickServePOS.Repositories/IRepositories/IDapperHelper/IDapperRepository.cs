using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.IRepositories.IDapperHelper
{
    public interface IDapperRepository
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql,object? parameter = null,CommandType commandType = CommandType.Text);

        Task<T?> QueryFirstOrDefaultAsync<T>(string sql,object? parameter = null,CommandType commandType = CommandType.Text);

        Task<int> ExecuteAsync(string sql,object? parameter = null,CommandType commandType = CommandType.Text);
    }
}
