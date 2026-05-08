using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Models.DTO.Profile
{
    public class UploadProfileImageDto
    {
        public IFormFile? Image { get; set; }
    }
}
