using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.ViewModel.Admin
{
    public class DashboardViewModel
    {
        public int TotalOrders { get; set; }

        public int PendingOrders { get; set; }

        public int TotalMenuItems { get; set; }

        public int TotalTables { get; set; }

        public int RunningTables { get; set; }

        public decimal TodayRevenue { get; set; }
    }
}
