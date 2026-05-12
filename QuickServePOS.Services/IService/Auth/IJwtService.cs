using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.IService.Auth
{
    public interface IJwtService
    {
        string GenerateToken(string userId,string email, string role);

        string GenerateRefreshToken();
    }
}
