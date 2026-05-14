using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Order;
using QuickServePOS.Models.ViewModel.Order;
using QuickServePOS.WebApp.HttpHelper;

namespace QuickServePOS.WebApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IApiHelper _apiHelper;

        public OrderController(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        // RUNNING TABLES
        public async Task<IActionResult> Index()
        {
            var response = await _apiHelper.GetAsync<List<RunningTableViewModel>>("TableAPI/GetAll");

            if (response == null)
            {
                return View(new List<RunningTableViewModel>());
            }

            return View(response);
        }

        // OPEN TABLE ORDER

        public async Task<IActionResult> OpenTable(int tableId)
        {
            var runningOrder = await _apiHelper.GetAsync<ApiDataResponse<OrderDetailsViewModel>>($"OrderAPI/RunningByTable/{tableId}");

            // Existing running order

            if (runningOrder != null && runningOrder.Success)
            {
                return RedirectToAction(nameof(Details),new { id = runningOrder.Data.Id });
            }

            // Create new order

            var createDto = new OrderCreateDto
            {
                TableId = tableId,
                OrderType = Models.Entities.Enums.OrderType.DineIn
            };

            var createResponse = await _apiHelper.PostDataAsync<OrderCreateDto, ApiResponse>("OrderAPI/Create",createDto);

            if (createResponse == null ||!createResponse.Success)
            {
                TempData["error"] =createResponse?.Message ?? "Unable to create order.";

                return RedirectToAction(nameof(Index));
            }

            // Reload running order

            var newOrder = await _apiHelper.GetAsync<ApiDataResponse<OrderDetailsViewModel>>($"OrderAPI/RunningByTable/{tableId}");

            if (newOrder == null || !newOrder.Success)
            {
                TempData["error"] ="Order created but unable to load.";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(
                nameof(Details),
                new { id = newOrder.Data.Id });
        }

        // ORDER DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var orderResponse =
                await _apiHelper.GetAsync<ApiDataResponse<OrderDetailsViewModel>>($"OrderAPI/Details/{id}");

            if (orderResponse == null ||
                !orderResponse.Success)
            {
                TempData["error"] = "Order not found.";

                return RedirectToAction(nameof(Index));
            }

            var categoryResponse =await _apiHelper.GetAsync<List<MenuCategoryViewModel>>("CategoryAPI/GetAll");

            var menuResponse =
                await _apiHelper.GetAsync<List<MenuItemCardViewModel>>("MenuItemAPI/GetAll");

            var vm = new POSOrderViewModel
            {
                Order = orderResponse.Data,
                Categories = categoryResponse ?? new(),
                MenuItems = menuResponse ?? new()
            };

            foreach (var item in vm.MenuItems)
            {
                item.OrderId = vm.Order.Id;
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(OrderItemCreateDto dto)
        {
            var response = await _apiHelper.PostDataAsync<OrderItemCreateDto, ApiResponse>("OrderAPI/AddItem",dto);

            return Json(response);
        }

        public async Task<IActionResult> GetCart(int orderId)
        {
            var response =
                await _apiHelper.GetAsync<ApiDataResponse<OrderDetailsViewModel>>
                    ($"OrderAPI/Cart/{orderId}");

            if (response == null || !response.Success)
            {
                return Content("");
            }

            return PartialView("_CartItemsPartialView",response.Data);
        }
    }
}
