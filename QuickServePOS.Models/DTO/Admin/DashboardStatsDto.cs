using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.Admin
{
    public class DashboardStatsDto
    {
        public int TotalStaff { get; set; }

        public int ActiveStaff { get; set; }

        public int DeletedStaff { get; set; }
    }
}
