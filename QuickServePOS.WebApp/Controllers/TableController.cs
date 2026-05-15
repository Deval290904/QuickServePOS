using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Floor;
using QuickServePOS.Models.DTO.RestaurantTable;
using QuickServePOS.Models.ViewModel.Table;
using QuickServePOS.WebApp.HttpHelper;


namespace QuickServePOS.WebApp.Controllers
{
    [Authorize(Roles = "Admin,Owner,Waiter,Cashier")]
    public class TableController : Controller
    {
        private readonly IApiHelper _apiHelper;
        private readonly IMapper _mapper;
        public TableController(IApiHelper apiHelper, IMapper mapper)
        {
            _apiHelper = apiHelper;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _apiHelper.GetAsync<List<TableListDto>>("TableAPI/GetAll");

            var model = _mapper.Map<List<TableListViewModel>>(response);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetTableList()
        {
            var response = await _apiHelper.GetAsync<List<TableListDto>>("TableAPI/GetAll");

            var model = _mapper.Map<List<TableListViewModel>>(response);

            ViewBag.IsTrash = false;

            return PartialView("_TableListPartialView",model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateTableViewModel();

            await LoadFloorDropdown(model);

            return PartialView("_CreateTablePartialView",model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
           CreateTableViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadFloorDropdown(model);

                return PartialView( "_CreateTablePartialView",model);
            }

            var dto = _mapper.Map<TableCreateDto>(model);

            var response = await _apiHelper.PostDataAsync<TableCreateDto, ApiResponse>("TableAPI/Create-Table", dto);

            if (response == null)
            {
                return Json(new{success = false,message = "Table creation failed."});}

            if (!response.Success)
            {
                return Json(new {success = false,message = response.Message});
            }

            return Json(new {success = true, message = response.Message});
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiHelper.GetAsync<TableUpdateDto>($"TableAPI/GetById/{id}");

            if (response == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<UpdateTableViewModel>(response);

            await LoadFloorDropdown(model);

            return PartialView("_EditTablePartialView",model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateTableViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadFloorDropdown(model);

                return PartialView("_EditTablePartialView",model);
            }

            var dto = _mapper.Map<TableUpdateDto>(model);

            var response = await _apiHelper.PutDataAsync<TableUpdateDto, ApiResponse>("TableAPI/Update-Table", dto);

            if (response == null)
            {
                return Json(new{success = false,message = "Table update failed."});
            }

            if (!response.Success)
            {
                return Json(new{success = false,message = response.Message});
            }

            return Json(new{success = true, message = response.Message});

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiHelper.DeleteAsync($"TableAPI/SoftDelete/{id}");

            if (response == null)
            {
                return Json(new{success = false, message = "Delete failed."});
            }

            if (!response.Success)
            {
                return Json(new{success = false, message = response.Message});
            }

            return Json(new{success = true, message = response.Message});
        }

        [HttpGet]
        public async Task<IActionResult> GetDeletedTables()
        {
            var response = await _apiHelper.GetAsync<List<TableListDto>>("TableAPI/TrashList");

            var model = _mapper.Map<List<TableListViewModel>>(response);

            ViewBag.IsTrash = true;

            return PartialView("_TableListPartialView", model);
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            var response = await _apiHelper.PutDataAsync<object, ApiResponse> ($"TableAPI/Restore/{id}", new { });

            if (response == null)
            {
                return Json(new{success = false,message = "Restore failed." });
            }

            if (!response.Success)
            {   
                return Json(new{ success = false, message = response.Message});
            }

            return Json(new{ success = true, message = response.Message });
        }

        private async Task LoadFloorDropdown(dynamic model)
        {
            var response = await _apiHelper.GetAsync<List<FloorListDto>>("FloorAPI/GetAll");

            model.Floors = response?
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList() ?? new List<SelectListItem>();
        }
    }
}

