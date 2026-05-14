using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Menu;
using QuickServePOS.Models.DTO.Order;
using QuickServePOS.Models.DTO.RestaurantTable;
using QuickServePOS.Models.ViewModel.Menu;
using QuickServePOS.Models.ViewModel.Order;
using QuickServePOS.Models.ViewModel.Table;
using QuickServePOS.WebApp.HttpHelper;

namespace QuickServePOS.WebApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IApiHelper _apiHelper;

        private readonly IMapper _mapper;

        public OrderController(IApiHelper apiHelper, IMapper mapper)
        {
            _apiHelper = apiHelper;
            _mapper = mapper;
        }

        // TABLE SCREEN
        public async Task<IActionResult> Index()
        {
            var response = await _apiHelper.GetAsync<List<TableListDto>>("TableAPI/GetAll");

            var model = _mapper.Map<List<TableListViewModel>>(response);

            return View(model);
        }

        public async Task<IActionResult> OpenTable(int tableId)
        {
            // Check existing order

            var existingOrder = await _apiHelper.GetAsync<ApiDataResponse<OrderDetailsDto>>($"OrderAPI/GetRunningByTable/{tableId}");

            // Existing running order

            if (existingOrder != null && existingOrder.Success && existingOrder.Data!=null)
            {
                return RedirectToAction(nameof(Details),
                    new
                    {
                        id = existingOrder.Data.Id
                    });
            }
            // Create new order

            var createResponse = await _apiHelper.PostDataAsync<object, ApiResponse>("OrderAPI/Create-Order",
                    new
                    {
                        tableId = tableId,
                        orderType = 1
                    });

            if (createResponse == null ||!createResponse.Success)
            {
                TempData["error"] =createResponse?.Message?? "Failed to create order.";

                return RedirectToAction(nameof(Index));
            }

            // Reload order

            var order = await _apiHelper.GetAsync<ApiDataResponse<OrderDetailsDto>>($"OrderAPI/GetRunningByTable/{tableId}");

            if (order == null || !order.Success || order.Data == null)
            {
                TempData["error"] = "Order not found.";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Details),new { id = order.Data.Id });
        }

        // ORDER DETAILS

        public async Task<IActionResult> Details(int id)
        {

            var response = await _apiHelper.GetAsync<ApiDataResponse<OrderDetailsDto>>($"OrderAPI/{id}");

            if (response == null || !response.Success || response.Data == null)
            {
                TempData["error"] = "Order not found.";

                return RedirectToAction(nameof(Index));
            }
            var categories = await _apiHelper.GetAsync<List<CategoryDto>>("CategoryAPI/GetAll");

            ViewBag.Categories = categories;

            var model = _mapper.Map<OrderDetailsViewModel>(response.Data);

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> GetMenuItemsByCategory(int categoryId)
        {
            var response = await _apiHelper.GetAsync<ApiDataResponse<List<MenuItemDto>>>($"MenuItemAPI/GetByCategory/{categoryId}");

            if (response == null ||!response.Success ||response.Data == null)
            {
                return PartialView("_MenuItemsPartialView",new List<MenuItemViewModel>());
            }

            var model = _mapper.Map<List<MenuItemViewModel>>(response.Data);

            return PartialView("_MenuItemsPartialView",model);
        }

        //[HttpPost]
        //public async Task<IActionResult> AddItem(AddItemViewModel model)
        //{
        //    var dto = _mapper.Map<OrderItemCreateDto>(model);

        //    var response = await _apiHelper.PostDataAsync<OrderItemCreateDto,ApiResponse>("OrderAPI/AddItem",dto);

        //    return Json(new
        //    {
        //        success = response?.Success,
        //        message = response?.Message
        //    });
        //}

        [HttpPost]
        public async Task<IActionResult> AddItem(AddItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Invalid request."
                });
            }

            // MAP DTO

            var dto = new OrderItemCreateDto
            {
                OrderId = model.OrderId,
                MenuItemId = model.MenuItemId,
                Quantity = model.Quantity,
                SpecialInstruction = model.SpecialInstruction

            };

            // API CALL

            var response = await _apiHelper
                .PostDataAsync<
                    OrderItemCreateDto,
                    ApiResponse>(
                        "OrderAPI/Add-Item",
                        dto);

            if (response == null ||
                !response.Success)
            {
                return Json(new
                {
                    success = false,
                    message = response?.Message
                              ?? "Failed to add item."
                });
            }

            return Json(new
            {
                success = true,
                message = response.Message
            });
        }

        [HttpGet]
        public async Task<IActionResult>LoadCart(int orderId)
        {
            var response = await _apiHelper.GetAsync<ApiDataResponse<OrderDetailsDto>>($"OrderAPI/{orderId}");

            if (response == null ||
                !response.Success ||
                response.Data == null)
            {
                return PartialView("_CartPartialView",new OrderDetailsViewModel());
            }

            var model = _mapper.Map<OrderDetailsViewModel>(response.Data);

            return PartialView("_CartPartialView",model);
        }
    }

}
