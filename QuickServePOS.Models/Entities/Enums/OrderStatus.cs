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

        Confirmed = 3,

        Preparing = 4,

        Ready = 5,

        Completed = 6,

        Cancelled = 7
    }
}
