using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuickServePOS.WebApp.Controllers
{
    [Authorize]
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return View("AdminDashboard");

            if (User.IsInRole("Owner"))
                return View("OwnerDashboard");

            if (User.IsInRole("Waiter"))
                return View("WaiterDashboard");

            if (User.IsInRole("Cashier"))
                return View("CashierDashboard");

            if (User.IsInRole("KitchenStaff"))
                return View("KitchenDashboard");

            if (User.IsInRole("Customer"))
                return View("CustomerDashboard");

            // fallback (optional safety)
            return View("AccessDenied");
        }
    }
}
