using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.Entities.Enums
{
    public enum KOTStatus
    {
        New = 1,
        Preparing = 2,
        Ready = 3,
        Served = 4,
        Cancelled = 5
    }
}
