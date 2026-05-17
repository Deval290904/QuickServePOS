using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.KOT;
using QuickServePOS.Models.Entities.Enums;
using QuickServePOS.Services.IService.KOT;


namespace QuickServePOS.WebAPI.Controllers
{
    [Authorize(Roles = "Admin,KitchenStaff,Waiter")]
    [Route("api/[controller]")]
    [ApiController]
    public class KOTAPIController : ControllerBase
    {
        private readonly IKOTService _kotService;

        public KOTAPIController(IKOTService kotService)
        {
            _kotService = kotService;
        }

        [HttpGet("kitchen-queue")]
        public async Task<IActionResult> GetKitchenQueue()
        {
            var result = await _kotService
                .GetKitchenQueueAsync();

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        [HttpGet("{kotId}")]
        public async Task<IActionResult>GetKOTDetails(int kotId)
        {
            var result = await _kotService.GetKOTByIdAsync(kotId);

            if (result == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "KOT not found."
                });
            }

            return Ok(new
            {
                success = true,
                data = result
            });
        }
        [HttpPut("{kotId}/status")]
        public async Task<IActionResult> UpdateKOTStatus(int kotId,[FromQuery] KOTStatus status)
        {
            await _kotService
                .UpdateKOTStatusAsync(
                    kotId,
                    status);

            return Ok(new
            {
                success = true,
                message = "KOT status updated successfully."
            });
        }

        [HttpPut("item-status")]
        public async Task<IActionResult>UpdateKOTItemStatus(UpdateKOTItemStatusDto dto)
        {
            await _kotService.UpdateKOTItemStatusAsync(dto.KOTItemId,Enum.Parse<KitchenItemStatus>(dto.Status));

            return Ok(new
            {
                success = true,
                message = "Item status updated successfully."
            });
        }

        [HttpGet("ready-queue")]
        public async Task<IActionResult>GetReadyQueue()
        {
            var readyKOTs =await _kotService.GetReadyKOTsAsync();

            return Ok(new ApiDataResponse<List<KitchenQueueDto>>
            {
                Success = true,

                Data = readyKOTs,

                Message ="Ready KOTs fetched successfully."
            });
        }

        [HttpPut("{kotId}/serve")]
        public async Task<IActionResult>ServeKOT(int kotId)
        {
            var result = await _kotService.ServeKOTAsync(kotId);

            if (!result)
            {
                return BadRequest(new ApiResponse{ Success = false, Message ="Failed to serve KOT."});
            }

            return Ok(new ApiResponse{Success = true,Message ="KOT served successfully."});
        }


    }
}
