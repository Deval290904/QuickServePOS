using Microsoft.AspNetCore.Http;

namespace QuickServePOS.Models.ViewModel.Profile
{
    public class UploadProfileImageViewModel
    {
        public IFormFile? Image { get; set; }
    }
}
