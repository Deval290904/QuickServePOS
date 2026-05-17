using QuickServePOS.Models.DTO.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.IRepositories
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalOrdersAsync();

        Task<int> GetPendingOrdersAsync();

        Task<int> GetTotalMenuItemsAsync();

        Task<int> GetTotalTablesAsync();

        Task<int> GetRunningTablesAsync();

        Task<decimal> GetTodayRevenueAsync();
    }
}
