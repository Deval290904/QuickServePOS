using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Menu;
using QuickServePOS.Models.ViewModel.Menu;
using QuickServePOS.WebApp.HttpHelper;

namespace QuickServePOS.WebApp.Controllers
{
    [Authorize(Roles = "Admin,Owner,Waiter,Cashier")]
    public class CategoryController : Controller
    {
        private readonly IApiHelper _apiHelper;
        private readonly IMapper _mapper;
        public CategoryController(IApiHelper apiHelper,IMapper mapper)
        {
            _apiHelper = apiHelper;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _apiHelper.GetAsync<List<CategoryDto>>("CategoryAPI/GetAll");

            var model = _mapper.Map<List<CategoryViewModel>>(response);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryList()
        {
            var response =await _apiHelper.GetAsync<List<CategoryDto>>("CategoryAPI/GetAll");

            var model = _mapper.Map<List<CategoryViewModel>>(response);

            ViewBag.IsTrash = false;

            return PartialView("_CategoryTablePartialView", model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateCategoryPartialView");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateCategoryPartialView", model);
            }

            var dto = _mapper.Map<CreateCategoryDto>(model);

            var response = await _apiHelper.PostDataAsync<CreateCategoryDto, ApiResponse>("CategoryAPI/Create-Category",dto);

            if (response == null)
            {
                return Json(new { success = false, message ="Category creation failed." });
            }
            if (!response.Success)
            {
                return Json(new { success = false, message = response?.Message ?? "Category creation failed." });
            }
            return Json(new{success = true,message = response.Message});
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response =await _apiHelper.GetAsync<CategoryDto>($"CategoryAPI/GetById/{id}");

            if (response == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<UpdateCategoryViewModel>(response);

            return PartialView("_EditCategoryPartialView", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditCategoryPartialView", model);
            }

            var dto = _mapper.Map<UpdateCategoryDto>(model);

            var response = await _apiHelper.PutDataAsync<UpdateCategoryDto, ApiResponse>("CategoryAPI/Update-Category", dto);

            if (response == null)
            {
                return Json(new { success = false, message = response?.Message ?? "Category update failed." });
            }

            return Json(new
            {
                success = true,
                message = response.Message
            });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiHelper.DeleteAsync($"CategoryAPI/SoftDelete/{id}");

            if (response == null)
            {
                return Json(new { success = false, message ="Delete failed." });
            }

            if(!response.Success)
            {
                return Json(new { success = false, message = response?.Message });
            }

            return Json(new{success = true,message = response.Message});
        }

        [HttpGet]
        public async Task<IActionResult> GetDeletedCategories()
        {
            var response =await _apiHelper.GetAsync<List<CategoryDto>>("CategoryAPI/TrashList");

            var model = _mapper.Map<List<CategoryViewModel>>(response);

            ViewBag.IsTrash = true;

            return PartialView( "_CategoryTablePartialView",model);
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            var response =await _apiHelper.PutDataAsync<object, ApiResponse>( $"CategoryAPI/Restore/{id}",new { });

            if (response == null)
            {
                return Json(new{success = false,message = response?.Message?? "Restore failed."});
            }
            if(!response.Success)
            {
                return Json(new { success = false, message = response?.Message ?? "Restore failed." });
            }

            return Json(new{success = true,message = response.Message});
        }

    }
}
