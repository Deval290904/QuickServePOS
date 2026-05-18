
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Menu;
using QuickServePOS.Models.ViewModel.Menu;
using QuickServePOS.WebApp.HttpHelper;

namespace QuickServePOS.WebApp.Controllers
{
    [Authorize(Roles = "Admin,Owner,Waiter,Cashier")]
    public class MenuItemController : Controller
    { 
        private readonly IApiHelper _apiHelper;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public MenuItemController(IApiHelper apiHelper, IMapper mapper, IConfiguration configuration    )
        {
            _apiHelper = apiHelper;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _apiHelper.GetAsync<List<MenuItemDto>>("MenuItemAPI/GetAll");
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"];

            if (response != null)
            {
                foreach (var item in response)
                {
                    if (!string.IsNullOrEmpty(item.ImageUrl))
                    {
                        item.ImageUrl =
                            apiBaseUrl!.TrimEnd('/')
                            + item.ImageUrl;
                    }
                }
            }

            var model =_mapper.Map<List<MenuItemViewModel>>(response);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetMenuItemList()
        {
            var response = await _apiHelper.GetAsync<List<MenuItemDto>>("MenuItemAPI/GetAll");
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"];

            if (response != null)
            {
                foreach (var item in response)
                {
                    if (!string.IsNullOrEmpty(item.ImageUrl))
                    {
                        item.ImageUrl =
                            apiBaseUrl!.TrimEnd('/')
                            + item.ImageUrl;
                    }
                }
            }

            var model = _mapper.Map<List<MenuItemViewModel>>(response);
            ViewBag.IsTrash = false;
            return PartialView("_MenuItemTablePartialView", model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories= await LoadCategoriesAsync();

            var model=new CreateMenuItemViewModel
            {
                Categories = categories
            };

            return PartialView("_CreateMenuItemPartialView", model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateMenuItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await LoadCategoriesAsync();
                return PartialView("_CreateMenuItemPartialView", model);
            }
            var dto = _mapper.Map<CreateMenuItemDto>(model);

            var response = await _apiHelper.PostFormDataAsync<CreateMenuItemDto,ApiResponse>("MenuItemAPI/Create-MenuItem",dto); 

            if (response == null)
            {
                return Json(new { success = false, message ="Menu item creation failed."});
            }

            if(!response.Success)
            {
                return Json(new { success = false, message = response?.Message});
            }
            return Json(new { success = true, message = response?.Message ?? "Menu item created successfully."});
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response =await _apiHelper.GetAsync<UpdateMenuItemDto>($"MenuItemAPI/GetById/{id}");

            if (response == null)
            {
                return NotFound();
            }

            var model =_mapper.Map<UpdateMenuItemViewModel>(response);

            model.Categories =await LoadCategoriesAsync();

            return PartialView("_EditMenuItemPartialView", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateMenuItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories =await LoadCategoriesAsync();

                return PartialView("_EditMenuItemPartialView", model);
            }

            var dto = _mapper.Map<UpdateMenuItemDto>(model);

            var response =await _apiHelper.PutFormDataAsync<UpdateMenuItemDto,ApiResponse>("MenuItemAPI/Update-MenuItem",dto);

            if (response == null)
            {
                return Json(new{success = false,message ="Menu item update failed."});
            }
            if(!response.Success)
            {
                return Json(new{success = false,message = response?.Message ?? "Menu item update failed."});
            }

            return Json(new{success = true,message = response?.Message ?? "Menu item updated successfully."});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response =await _apiHelper.DeleteAsync($"MenuItemAPI/SoftDelete/{id}");

            if (response == null)
            {
                return Json(new{success = false,message ="Delete failed."});
            }

            if(!response.Success)
            {
                return Json(new{success = false,message = response?.Message ?? "Delete failed."});
            }

            return Json(new{success = true,message = response?.Message ?? "Delete successful."});
        }

        [HttpGet]
        public async Task<IActionResult> GetDeletedMenuItems()
        {
            var response =
                await _apiHelper
                .GetAsync<List<MenuItemDto>>(
                    "MenuItemAPI/TrashList");

            var apiBaseUrl =
                _configuration["ApiSettings:BaseUrl"];

            if (response != null)
            {
                foreach (var item in response)
                {
                    if (!string.IsNullOrEmpty(item.ImageUrl))
                    {
                        item.ImageUrl =
                            apiBaseUrl!.TrimEnd('/')
                            + item.ImageUrl;
                    }
                }
            }

            var model =
                _mapper.Map<List<MenuItemViewModel>>(
                    response);

            ViewBag.IsTrash = true;

            return PartialView(
                "_MenuItemTablePartialView",
                model);
        }
       

        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            var response =await _apiHelper.PutDataAsync<object, ApiResponse>($"MenuItemAPI/Restore/{id}", new { });

            if (response == null || !response.Success)
            {
                return Json(new
                {
                    success = false,
                    message = response?.Message
                              ?? "Restore failed."
                });
            }

            return Json(new
            {
                success = true,
                message = response.Message
            });
        }


        private async Task<IEnumerable<SelectListItem>>LoadCategoriesAsync()
        {
            var response =await _apiHelper.GetAsync<List<CategoryDto>>("CategoryAPI/GetAll");

            return response!
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });
        }
    }
}
