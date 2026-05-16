using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.KOT;
using QuickServePOS.Models.Entities.Enums;
using QuickServePOS.Services.IService.KOT;


namespace QuickServePOS.WebAPI.Controllers
{
    [Authorize(Roles = "Admin,KitchenStaff")]
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


    }
}
