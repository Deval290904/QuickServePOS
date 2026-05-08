using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.ViewModel;
using QuickServePOS.WebApp.HttpHelper;

namespace QuickServePOS.WebApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IApiHelper _apiHelper;

        public ProfileController(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ProfileEdit()
        {
            var result = await _apiHelper.GetAsync<ApiDataResponse<ProfileUpdateViewModel>>("ProfileAPI/GetProfile");

            if(result==null)
            {
                TempData["Error"] = "Unable to load profile.";
                return RedirectToAction("Index","DashBoard");
            }
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> ProfileEdit(ProfileUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _apiHelper.PutDataAsync<ProfileUpdateViewModel,ApiResponse>("ProfileAPI/UpdateProfile", model);

            if (response == null || !response.Success)
            {
                TempData["Error"]= response?.Message ?? "Unable to update profile.";
                return View(model);
            }

            TempData["Success"]=response?.Message ?? "Profile updated successfully.";
            return RedirectToAction(nameof(ProfileEdit));
        }
    }
}
