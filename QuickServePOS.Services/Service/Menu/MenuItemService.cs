using AutoMapper;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Menu;
using QuickServePOS.Models.Entities.Menu;
using QuickServePOS.Repositories.IUnitofWork;
using QuickServePOS.Services.IService.Common;
using QuickServePOS.Services.IService.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.Service.Menu
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IImageService _imageService;

        public MenuItemService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<List<MenuItemDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.MenuItems.GetAllAsync();

            return _mapper.Map<List<MenuItemDto>>(entities);
        }

        public async Task<MenuItemDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.MenuItems.GetByIdAsync(id);

            if (entity == null)
            {
                return null;
            }

            return _mapper.Map<MenuItemDto>(entity);
        }

        public async Task<ApiResponse> CreateAsync(CreateMenuItemDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);

            if (category == null)
            {
                return Fail("Category not found.");
            }

            var exists = await _unitOfWork.MenuItems.ExistsAsync(dto.Name,dto.CategoryId);

            if (exists)
            {
                return Fail("Menu item already exists.");
            }

            var entity = _mapper.Map<MenuItemEntity>(dto);

            // IMAGE UPLOAD

            if (dto.ImageFile != null)
            {
                entity.ImageUrl = await _imageService.UploadImageAsync(dto.ImageFile,"menuitems");
            }

            await _unitOfWork.MenuItems.AddAsync(entity);

            await _unitOfWork.SaveChangesAsync();

            return Success("Menu item created successfully.");
        }

        public async Task<ApiResponse> UpdateAsync(UpdateMenuItemDto dto)
        {
            var entity = await _unitOfWork.MenuItems.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                return Fail("Menu item not found.");
            }

            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);

            if (category == null)
            {
                return Fail("Category not found.");
            }

            var exists = await _unitOfWork.MenuItems.ExistsAsync(dto.Name,dto.CategoryId, dto.Id);

            if (exists)
            {
                return Fail("Menu item already exists.");
            }

            _mapper.Map(dto, entity);

            // IMAGE UPDATE

            if (dto.ImageFile != null)
            {
                // Delete old image

                if (!string.IsNullOrEmpty(entity.ImageUrl))
                {
                    _imageService.DeleteImage(entity.ImageUrl);
                }

                // Upload new image

                entity.ImageUrl = await _imageService.UploadImageAsync(dto.ImageFile,"menuitems");
            }

            _unitOfWork.MenuItems.Update(entity);

            await _unitOfWork
                .SaveChangesAsync();

            return Success("Menu item updated successfully.");
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.MenuItems.GetByIdAsync(id);

            if (entity == null)
            {
                return Fail("Menu item not found.");
            }

            _unitOfWork.MenuItems.Delete(entity);

            await _unitOfWork.SaveChangesAsync();

            return Success("Menu item deleted successfully.");
        }

        public async Task<List<MenuItemDto>> GetDeletedAsync()
        {
            var entities = await _unitOfWork.MenuItems.GetDeletedMenuItemsAsync();

            return _mapper.Map<List<MenuItemDto>>(entities);
        }

        public async Task<ApiResponse> RestoreAsync(int id)
        {
            var entity = await _unitOfWork.MenuItems.GetByIdIgnoreQueryFilterAsync(id);

            if (entity == null)
            {
                return Fail("Menu item not found.");
            }

            entity.IsDeleted = false;

            entity.DeletedAt = null;

            _unitOfWork.MenuItems.Update(entity);

            await _unitOfWork.SaveChangesAsync();

            return Success("Menu item restored successfully.");
        }

        private ApiResponse Success(string message)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message
            };
        }

        private ApiResponse Fail(string message)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message
            };
        }

        public async Task<ApiDataResponse<List<MenuItemDto>>>GetByCategoryAsync(int categoryId)
        {
            var items = await _unitOfWork.MenuItems.GetByCategoryAsync(categoryId);

            var data = _mapper.Map<List<MenuItemDto>>(items);

            return new ApiDataResponse<List<MenuItemDto>>
            {
                Success = true,
                Data = data
            };
        }
    }
}
