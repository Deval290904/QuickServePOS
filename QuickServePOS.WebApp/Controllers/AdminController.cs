using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickServePOS.Models.DTO.Admin;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.ViewModel.Authentication;
using QuickServePOS.Models.ViewModel.Profile;
using QuickServePOS.WebApp.HttpHelper;
using System.Net.Http.Json;

namespace QuickServePOS.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IApiHelper _apiHelper;
        private readonly IMapper _mapper;

        public AdminController(IApiHelper apiHelper, IMapper mapper)
        {
            _apiHelper = apiHelper;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        private async Task<List<SelectListItem>> GetRolesAsync()
        {
            var roles = await _apiHelper.GetAsync<List<string>>("AdminAPI/RolesList") ?? new List<string>();

            return roles.Select(r => new SelectListItem
            {
                Value = r,
                Text = r
            }).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> CreateStaff()
        {
            var vm = new CreateStaffViewModel
            {
                Roles = await GetRolesAsync()
            };
            return PartialView("_CreateStaffPartialView", vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStaff(CreateStaffViewModel vm)
        {
            vm.Roles = await GetRolesAsync();

            if (!ModelState.IsValid)
            {
                return PartialView("_CreateStaffPartialView", vm);
            }

            var dto = _mapper.Map<CreateStaffAccountDto>(vm);

            var result = await _apiHelper.PostAsync("AdminAPI/Create-Staff", dto);

            if (result==null)
            {
                return Json(new { success = false, message = result?.Message ?? "Staff creation failed." });
            }
            if (!result.Success)
            {
                return Json(new{ success = false, message = result.Message});
            }
            return Json(new { success = true, message= result.Message });

        }

        [HttpGet]
        public async Task<IActionResult> StaffList()
        {
            var data = await _apiHelper.GetAsync<List<StaffListViewModel>>("AdminAPI/StaffList");

            var stats = await _apiHelper.GetAsync<DashboardStatsViewModel>("AdminAPI/Staff-Stats");

            ViewBag.Stats = stats;

            return View(data ?? new List<StaffListViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> GetStaffList()
        {
            var data = await _apiHelper.GetAsync<List<StaffListViewModel>>("AdminAPI/StaffList");
            ViewBag.IsTrash = false;
            return PartialView("_StaffTablePartialView", data ?? new List<StaffListViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> EditStaff(string id)
        {
            var data = await _apiHelper.GetAsync<UpdateStaffViewModel>($"AdminAPI/Get-Staff/{id}");

            if (data == null)
                return NotFound();

            data.Roles = await GetRolesAsync();

            return PartialView("_EditStaffPartialView", data);
        }

        [HttpPost]
        public async Task<IActionResult> EditStaff(UpdateStaffViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Roles = await GetRolesAsync();
                return PartialView("_EditStaffPartialView", vm);
            }

           var dto= _mapper.Map<UpdateStaffDto>(vm);

            var result = await _apiHelper.PutAsync("AdminAPI/Update-Staff", dto);

            return Json(new{success = true,message = result.Message});
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStaff(string id)
        {
            var result = await _apiHelper.DeleteAsync($"AdminAPI/Delete-Staff/{id}");
            if (!result.Success)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true, message = "Staff deleted successfully" });
        }
          
        [HttpPost]
        public async Task<IActionResult> RestoreStaff(string id)
        {
            var result = await _apiHelper.PutAsync($"AdminAPI/Restore-Staff/{id}");

            if (!result.Success)
                return Json(new { success = false, message = result.Message });

            return Json(new { success = true, message = result.Message });
        }

        [HttpGet]
        public async Task<IActionResult> GetDeletedStaff()
        {
            var data = await _apiHelper.GetAsync<List<StaffListViewModel>>("AdminAPI/Deleted-Staff-List");
            ViewBag.IsTrash = true;
            return PartialView("_StaffTablePartialView", data ?? new List<StaffListViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> DeletePermanentStaff(string id)
        {
            var result = await _apiHelper.DeleteAsync($"AdminAPI/Delete-Permanent/{id}");

            if (!result.Success)
                return Json(new { success = false, message = result.Message });

            return Json(new { success = result.Success, message = result.Message });
        }

        public async Task<IActionResult> GetStaffStats()
        {
            var data = await _apiHelper.GetAsync<DashboardStatsViewModel>("AdminAPI/Staff-Stats");

            return PartialView("_StaffStatsPartialView", data);
        }
    }
}