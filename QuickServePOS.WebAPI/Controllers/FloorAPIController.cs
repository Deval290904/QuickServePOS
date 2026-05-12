using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Floor;
using QuickServePOS.Services.IService.Table;

namespace QuickServePOS.WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class FloorAPIController : ControllerBase
    {
        private readonly IFloorService _floorService;

        public FloorAPIController(IFloorService floorService)
        {
            _floorService = floorService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _floorService.GetAllAsync();

            return Ok(data);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data=await _floorService.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Floor not found."
                });
            }

            return Ok(data);
        }

        [HttpPost("Create-Floor")]
        public async Task<IActionResult> Create(FloorCreateDto dto)
        {
            var result = await _floorService.CreateAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("Update-Floor")]
        public async Task<IActionResult> Update(FloorUpdateDto dto)
        {
            var result = await _floorService.UpdateAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _floorService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("TrashList")]
        public async Task<IActionResult> Trash()
        {
            var data = await _floorService.GetDeletedAsync();

            return Ok(data);
        }

        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var result = await _floorService.RestoreAsync(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
