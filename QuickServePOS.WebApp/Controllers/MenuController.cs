using Microsoft.AspNetCore.Mvc;

namespace QuickServePOS.WebApp.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
