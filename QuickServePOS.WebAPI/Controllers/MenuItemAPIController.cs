using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Menu;
using QuickServePOS.Models.Entities.Menu;
using QuickServePOS.Repositories.IUnitofWork;
using QuickServePOS.Services.IService.Common;
using QuickServePOS.Services.IService.Menu;

namespace QuickServePOS.WebAPI.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemAPIController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemAPIController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _menuItemService.GetAllAsync();

            return Ok(data);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _menuItemService.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Menu item not found."
                });
            }

            return Ok(data);
        }

        [HttpPost("Create-MenuItem")]
        public async Task<IActionResult> Create([FromForm] CreateMenuItemDto dto)
        {
            var result = await _menuItemService
                .CreateAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("Update-MenuItem")]
        public async Task<IActionResult> Update([FromForm] UpdateMenuItemDto dto)
        {
            var result = await _menuItemService
                .UpdateAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _menuItemService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("TrashList")]
        public async Task<IActionResult> Trash()
        {
            var data = await _menuItemService.GetDeletedAsync();

            return Ok(data);
        }

        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var result = await _menuItemService.RestoreAsync(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("GetByCategory/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var response = await _menuItemService.GetByCategoryAsync(categoryId);

            return Ok(response);
        }

    }
}
