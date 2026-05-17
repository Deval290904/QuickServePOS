using QuickServePOS.Models.DTO.Admin;
using QuickServePOS.Repositories.IRepositories;
using QuickServePOS.Repositories.IUnitofWork;
using QuickServePOS.Services.IService.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.Service.Admin
{

    public class DashboardService : IDashboardService   
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            return new DashboardDto
            {
                TotalOrders = await _unitOfWork
                    .Dashboard
                    .GetTotalOrdersAsync(),

                PendingOrders = await _unitOfWork
                    .Dashboard
                    .GetPendingOrdersAsync(),

                TotalMenuItems = await _unitOfWork
                    .Dashboard
                    .GetTotalMenuItemsAsync(),

                TotalTables = await _unitOfWork
                    .Dashboard
                    .GetTotalTablesAsync(),

                RunningTables = await _unitOfWork
                    .Dashboard
                    .GetRunningTablesAsync(),

                TodayRevenue = await _unitOfWork
                    .Dashboard
                    .GetTodayRevenueAsync()
            };
        }
    }
}
