using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuickServePOS.Models.ViewModel
{
    public class CreateStaffViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Phone { get; set; }
       
        public string Role { get; set; }

        // 👇 For dropdown
        public List<SelectListItem>? Roles { get; set; }
    }
}
