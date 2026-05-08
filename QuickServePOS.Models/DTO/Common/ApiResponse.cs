using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public object Message { get; set; }
    }
}
