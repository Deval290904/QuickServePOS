using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Floor;
using QuickServePOS.Models.ViewModel.Table;
using QuickServePOS.WebApp.HttpHelper;

namespace QuickServePOS.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FloorController : Controller
    {
        private readonly IApiHelper _apiHelper;

        private readonly IMapper _mapper;

        public FloorController(IApiHelper apiHelper, IMapper mapper)
        {
            _apiHelper = apiHelper;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _apiHelper.GetAsync<List<FloorListDto>>("FloorAPI/GetAll");

            var model = _mapper.Map<List<FloorViewModel>>(response);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetFloorList()
        {
            var response = await _apiHelper.GetAsync<List<FloorListDto>>("FloorAPI/GetAll");

            var model = _mapper .Map<List<FloorViewModel>>(response);

            ViewBag.IsTrash = false;

            return PartialView("_FloorTablePartialView",model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateFloorPartialView");
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateFloorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateFloorPartialView",model);
            }

            var dto = _mapper.Map<FloorCreateDto>(model);

            var response = await _apiHelper.PostDataAsync<FloorCreateDto, ApiResponse>("FloorAPI/Create-Floor",dto);

            if (response == null)
            {
                return Json(new{success = false,message = "Floor creation failed."});
            }

            if (!response.Success)
            {
                return Json(new{success = false,message = response.Message});
            }

            return Json(new{success = true,message = response.Message});
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiHelper.GetAsync<FloorUpdateDto>($"FloorAPI/GetById/{id}");

            if (response == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<UpdateFloorViewModel>(response);

            return PartialView("_EditFloorPartialView",model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(
           UpdateFloorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditFloorPartialView",model);
            }

            var dto = _mapper.Map<FloorUpdateDto>(model);

            var response = await _apiHelper.PutDataAsync<FloorUpdateDto, ApiResponse>("FloorAPI/Update-Floor",dto);

            if (response == null)
            {
                return Json(new{success = false,message = "Floor update failed."});
            }

            if (!response.Success)
            {
                return Json(new{success = false,message = response.Message});   
            }

            return Json(new{success = true,message = response.Message});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiHelper.DeleteAsync($"FloorAPI/SoftDelete/{id}");

            if (response == null)
            {
                return Json(new{success = false,message = "Delete failed."});
            }

            if (!response.Success)
            {
                return Json(new{success = false,message = response.Message});
            }

            return Json(new{success = true,message = response.Message});
        }

        [HttpGet]
        public async Task<IActionResult> GetDeletedFloors()
        {
            var response = await _apiHelper.GetAsync<List<FloorListDto>>("FloorAPI/TrashList");

            var model = _mapper.Map<List<FloorViewModel>>(response);

            ViewBag.IsTrash = true;

            return PartialView( "_FloorTablePartialView", model);
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            var response = await _apiHelper.PutDataAsync<object, ApiResponse>($"FloorAPI/Restore/{id}",new { });

            if (response == null)
            {
                return Json(new{success = false,message = "Restore failed."});
            }

            if (!response.Success)
            {
                return Json(new {success = false, message = response.Message});
            }

            return Json(new {success = true, message = response.Message});
        }

    }
}
