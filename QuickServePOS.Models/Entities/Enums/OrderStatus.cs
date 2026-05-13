using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.Entities.Enums
{
    public enum OrderStatus
    {
        Draft = 1,
        Running = 2,
        Completed = 3,
        Cancelled = 4,
        Paid = 5
    }
}
