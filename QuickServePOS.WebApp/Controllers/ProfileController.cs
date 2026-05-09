using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Profile;
using QuickServePOS.Models.ViewModel.Profile;
using QuickServePOS.WebApp.HttpHelper;

namespace QuickServePOS.WebApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IApiHelper _apiHelper;
        private readonly IConfiguration _configuration;

        public ProfileController(IApiHelper apiHelper, IConfiguration configuration)
        {
            _apiHelper = apiHelper;
            _configuration = configuration;
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
            var apiBaseUrl =_configuration["ApiSettings:BaseUrl"];

            if (!string.IsNullOrEmpty(result.Data.ProfileImagePath))
            {
                result.Data.ProfileImagePath = apiBaseUrl.TrimEnd('/') +
                    result.Data.ProfileImagePath;
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
                TempData["Error"] = response?.Message?.ToString() ?? "Unable to update profile.";
                return View(model);
            }

            TempData["Success"]=response?.Message?.ToString() ?? "Profile updated successfully.";
            return RedirectToAction(nameof(ProfileEdit));
        }


        [HttpPost]
        public async Task<IActionResult> UploadPhoto(ProfileUpdateViewModel model)
        {
            if (model.Image == null)
            {
                TempData["Error"] =
                    "Please select a photo.";

                return RedirectToAction(nameof(ProfileEdit));
            }

            var imageDto = new UploadProfileImageViewModel
            {
                Image = model.Image
            };

            var response = await _apiHelper.PostFormDataAsync<UploadProfileImageViewModel, ApiResponse>(
                        "ProfileAPI/UploadProfileImage",
                        imageDto);

            if (response == null || !response.Success)
            {
                TempData["Error"] =response?.Message?.ToString()?? "Unable to upload photo.";

                return RedirectToAction(nameof(ProfileEdit));
            }

            TempData["Success"] ="Profile photo uploaded successfully.";

            return RedirectToAction(nameof(ProfileEdit));
        }
    }
}
