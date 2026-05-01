using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.Auth
{
    public class AuthResponseDto
    {
        public string Message { get; set; }
        public string AccessToken { get; set; }
    }
}
