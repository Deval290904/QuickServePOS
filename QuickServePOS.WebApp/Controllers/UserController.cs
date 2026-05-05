using Microsoft.AspNetCore.Mvc;

namespace QuickServePOS.WebApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
