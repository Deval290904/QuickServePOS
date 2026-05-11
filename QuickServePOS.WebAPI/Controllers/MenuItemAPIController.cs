using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Menu;
using QuickServePOS.Models.Entities.Menu;
using QuickServePOS.Repositories.IUnitofWork;
using QuickServePOS.Services.IService;

namespace QuickServePOS.WebAPI.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IImageService _imageService;

        public MenuItemAPIController(IUnitOfWork unitOfWork,IMapper mapper,IImageService imageService)
        {
            _unitOfWork = unitOfWork;

            _mapper = mapper;

            _imageService = imageService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var menuItems =await _unitOfWork.MenuItems.GetAllAsync();

            var data =
                _mapper.Map<List<MenuItemDto>>(menuItems);

            return Ok(data);
        }

        // GET BY ID
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var menuItem =
                await _unitOfWork.MenuItems.GetByIdAsync(id);

            if (menuItem == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Menu item not found."
                });
            }

            var data = _mapper.Map<MenuItemDto>(menuItem);

            return Ok(data);
        }

        [HttpPost("Create-MenuItem")]
        public async Task<IActionResult> Create([FromForm] CreateMenuItemDto dto)
        {
            var exists =
                await _unitOfWork.MenuItems.ExistsAsync(
                    dto.Name,
                    dto.CategoryId);

            if (exists)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Menu item already exists."
                });
            }

            var entity =
                _mapper.Map<MenuItemEntity>(dto);

            // IMAGE UPLOAD
            if (dto.ImageFile != null)
            {
                entity.ImageUrl =
                    await _imageService
                    .UploadImageAsync(dto.ImageFile, "menuitems");
            }

            await _unitOfWork.MenuItems.AddAsync(entity);

            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Menu item created successfully."
            });
        }

        [HttpPut("Update-MenuItem")]
        public async Task<IActionResult> Update([FromForm] UpdateMenuItemDto dto)
        {
            var menuItem =
                await _unitOfWork.MenuItems
                .GetByIdAsync(dto.Id);

            if (menuItem == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Menu item not found."
                });
            }

            var exists =
                await _unitOfWork.MenuItems
                .ExistsAsync(
                    dto.Name,
                    dto.CategoryId,
                    dto.Id);

            if (exists)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Menu item already exists."
                });
            }

            _mapper.Map(dto, menuItem);

            // IMAGE UPDATE
            if (dto.ImageFile != null)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(menuItem.ImageUrl))
                {
                    _imageService.DeleteImage(menuItem.ImageUrl);
                }

                // Upload new image
                menuItem.ImageUrl =
                    await _imageService
                    .UploadImageAsync(
                        dto.ImageFile,
                        "menuitems");
            }

            menuItem.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.MenuItems.Update(menuItem);

            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Menu item updated successfully."
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(id);

            if (menuItem == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Menu item not found."
                });
            }

            _unitOfWork.MenuItems.Delete(menuItem);

            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Menu item deleted successfully."
            });
        }

    }
}
