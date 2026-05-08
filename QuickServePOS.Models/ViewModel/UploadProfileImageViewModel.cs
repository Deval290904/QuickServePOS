using Microsoft.AspNetCore.Http;

namespace QuickServePOS.Models.ViewModel
{
    public class UploadProfileImageViewModel
    {
        public IFormFile? Image { get; set; }
    }
}
