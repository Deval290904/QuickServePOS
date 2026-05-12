using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.RestaurantTable;
using QuickServePOS.Services.IService.Table;

namespace QuickServePOS.WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TableAPIController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TableAPIController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _tableService.GetAllAsync();

            return Ok(data);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _tableService.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Table not found."
                });
            }

            return Ok(data);
        }

        [HttpPost("Create-Table")]
        public async Task<IActionResult> Create(TableCreateDto dto)
        {
            var result = await _tableService.CreateAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("Update-Table")]
        public async Task<IActionResult> Update(TableUpdateDto dto)
        {
            var result = await _tableService.UpdateAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tableService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("TrashList")]
        public async Task<IActionResult> Trash()
        {
            var data = await _tableService
                .GetDeletedAsync();

            return Ok(data);
        }

        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var result = await _tableService.RestoreAsync(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
