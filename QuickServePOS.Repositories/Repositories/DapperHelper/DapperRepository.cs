using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuickServePOS.Repositories.IRepositories.IDapperHelper;
using System.Data;

namespace QuickServePOS.Repositories.Repositories.DapperHelper
{
    public class DapperRepository : IDapperRepository
    {
        private readonly IConfiguration _configuration;

        public DapperRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            object? parameter = null,
            CommandType commandType = CommandType.Text)
        {
            using var connection = CreateConnection();

            return await connection.QueryAsync<T>(
                sql,
                parameter,
                commandType: commandType);
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(
            string sql,
            object? parameter = null,
            CommandType commandType = CommandType.Text)
        {
            using var connection = CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<T>(
                sql, 
                parameter,
                commandType: commandType);
        }

        public async Task<int> ExecuteAsync(
            string sql,
            object? parameter = null,
            CommandType commandType = CommandType.Text)
        {
            using var connection = CreateConnection();

            return await connection.ExecuteAsync(
                sql,
                parameter,
                commandType: commandType);
        }
    }
}
