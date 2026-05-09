using System.ComponentModel.DataAnnotations;

namespace QuickServePOS.Models.ViewModel.Authentication
{
    public class LoginViewModel
    {
        
        public string Email { get; set; }
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
