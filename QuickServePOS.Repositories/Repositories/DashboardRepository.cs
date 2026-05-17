
using Microsoft.EntityFrameworkCore;
using QuickServePOS.DbContextData.Data;
using QuickServePOS.Models.Entities.Enums;
using QuickServePOS.Repositories.IRepositories;

namespace QuickServePOS.Repositories.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _AppDbcontext;

        public DashboardRepository(AppDbContext context)
        {
            _AppDbcontext = context;
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            return await _AppDbcontext.Orders.CountAsync();
        }

        public async Task<int> GetPendingOrdersAsync()
        {
            return await _AppDbcontext.Orders
                .CountAsync(x =>
                     x.Status != OrderStatus.Completed &&
                     x.Status != OrderStatus.Cancelled);
        }

        public async Task<int> GetTotalMenuItemsAsync()
        {
            return await _AppDbcontext.MenuItems
                .CountAsync();
        }

        public async Task<int> GetTotalTablesAsync()
        {
            return await _AppDbcontext.RestaurantTables
                .CountAsync();
        }

        public async Task<int> GetRunningTablesAsync()
        {
            return await _AppDbcontext.RestaurantTables
                .CountAsync(x =>
                    x.Status == TableStatus.Occupied);
        }

        public async Task<decimal> GetTodayRevenueAsync()
        {
            var today = DateTime.Today;

            return await _AppDbcontext.Orders
                .Where(x =>
                    x.CreatedAt.Date == today &&
                    x.Status == OrderStatus.Completed)
                .SumAsync(x =>
                    (decimal?)x.TotalAmount) ?? 0;
        }

    }
}
