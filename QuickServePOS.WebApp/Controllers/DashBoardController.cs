using Microsoft.AspNetCore.Mvc;

namespace QuickServePOS.WebApp.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
