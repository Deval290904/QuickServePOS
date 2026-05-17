using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.KOT;
using QuickServePOS.Models.ViewModel.KOT;
using QuickServePOS.WebApp.HttpHelper;

namespace QuickServePOS.WebApp.Controllers
{
    [Authorize(Roles ="Admin,KitchenStaff,Waiter")]
    public class KitchenController : Controller
    {
        private readonly IApiHelper _apiHelper;
        private readonly IMapper _mapper;

        public KitchenController(IApiHelper apiHelper, IMapper mapper)
        {
            _apiHelper = apiHelper;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response =await _apiHelper.GetAsync<ApiDataResponse<List<KitchenQueueDto>>>("KOTAPI/kitchen-queue");

            if (response == null)
            {
                return View(new List<KitchenQueueViewModel>());
            }

            var viewModel =_mapper.Map<List<KitchenQueueViewModel>>(response?.Data ?? new());

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetKitchenQueuePartial()
        {
            var response =await _apiHelper.GetAsync<ApiDataResponse<List<KitchenQueueDto>>>("KOTAPI/kitchen-queue");

            var viewModel =_mapper.Map<List<KitchenQueueViewModel>>(response?.Data ?? new());

            return PartialView("_KitchenQueuePartial",viewModel);
        }

        [HttpGet]
        public async Task<IActionResult>Details(int id)
        {
            var response =await _apiHelper.GetAsync<ApiDataResponse<KOTDetailsDto>>( $"KOTAPI/{id}");

            if (response == null || response.Data==null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<KOTDetailsViewModel>(response.Data);

            return PartialView("_KOTDetailsPartial", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult>UpdateStatus(int kotId,string status)
        {
            var result =await _apiHelper.PutAsync($"KOTAPI/{kotId}/status?status={status}");

            if (!result.Success)
            {
                return Json(new{ success = false,message = "Status update failed."});
            }

            return Json(new{ success = true,message = "KOT status updated successfully." });
        }
        [HttpPost]
        public async Task<IActionResult>UpdateItemStatus(UpdateKOTItemStatusViewModel vm)
        {
            var dto =_mapper.Map<UpdateKOTItemStatusDto>(vm);

            var result =await _apiHelper.PutDataAsync<UpdateKOTItemStatusDto,ApiResponse>("KOTAPI/item-status",dto);

            if (result == null || !result.Success)
            {
                return Json(new{success = false,message = "Status update failed."});
            }

            return Json(new {success = true,message ="Item status updated successfully." });
        }

        [HttpGet]
        public async Task<IActionResult>ReadyQueue()
        {
            var response = await _apiHelper.GetAsync<ApiDataResponse<List<KitchenQueueDto>>>("KOTAPI/ready-queue");

            var viewModel = _mapper.Map< List<KitchenQueueViewModel>>( response?.Data ?? new());

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult>ReadyQueuePartial()
        {
            var response =
                await _apiHelper.GetAsync<ApiDataResponse<List<KitchenQueueDto>>>("KOTAPI/ready-queue");

            var viewModel =_mapper.Map<List<KitchenQueueViewModel>>(response?.Data ?? new());

            return PartialView("_ReadyQueuePartial",viewModel);
        }

        [HttpPost]
        public async Task<IActionResult>ServeKOT(int kotId)
        {
            var result = await _apiHelper.PutAsync($"KOTAPI/{kotId}/serve");

            if (!result.Success)
            {
                return Json(new{success = false, message ="Failed to serve KOT."});
            }

            return Json(new{success = true,message = "KOT served successfully." });
        }
    }
}
