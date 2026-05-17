using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.ViewModel.Admin;
using QuickServePOS.WebApp.HttpHelper;

namespace QuickServePOS.WebApp.Controllers
{
    [Authorize]
    public class DashBoardController : Controller
    {
        private readonly IApiHelper _apiHelper;

        public DashBoardController(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var model = await _apiHelper.GetAsync<DashboardViewModel>("DashBoardAPI/summary");

                return View("AdminDashboard", model);
               
            }
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
