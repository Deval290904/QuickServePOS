using Microsoft.AspNetCore.Mvc;

namespace QuickServePOS.WebApp.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
