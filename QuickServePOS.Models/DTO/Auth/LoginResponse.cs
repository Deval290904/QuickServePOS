using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.Auth
{
    public class LoginResponse
    {
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
