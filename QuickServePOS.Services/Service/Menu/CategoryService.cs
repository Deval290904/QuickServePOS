using AutoMapper;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Menu;
using QuickServePOS.Models.Entities.Menu;
using QuickServePOS.Repositories.IRepositories;
using QuickServePOS.Repositories.IUnitofWork;
using QuickServePOS.Services.IService.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.Service.Menu
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
                return null;

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<ApiResponse> CreateAsync(CreateCategoryDto dto)
        {
            var exists = await _unitOfWork.Categories.ExistsAsync(dto.Name);

            if (exists)
            {
                return Fail("Category name already exists.");
            }

            var category = _mapper.Map<CategoryEntity>(dto);

            await _unitOfWork.Categories.AddAsync(category);

            await _unitOfWork.SaveChangesAsync();

            return Success("Category created successfully.");
        }

        public async Task<ApiResponse> UpdateAsync(UpdateCategoryDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(dto.Id);

            if (category == null)
            {
                return Fail("Category not found.");
            }

            var exists = await _unitOfWork.Categories.ExistsAsync(dto.Name, dto.Id);

            if (exists)
            {
                return Fail("Category name already exists.");
            }

            _mapper.Map(dto, category);

            _unitOfWork.Categories.Update(category);

            await _unitOfWork.SaveChangesAsync();

            return Success("Category updated successfully.");
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                return Fail("Category not found.");
            }

            if (category.MenuItems.Any(x => !x.IsDeleted))
            {
                return Fail("Cannot delete category with active menu items.");
            }

            _unitOfWork.Categories.Delete(category);

            await _unitOfWork.SaveChangesAsync();

            return Success("Category deleted successfully.");
        }

        public async Task<List<CategoryDto>> GetDeletedAsync()
        {
            var categories = await _unitOfWork.Categories.GetDeletedCategoriesAsync();

            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<ApiResponse> RestoreAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdIgnoreQueryFilterAsync(id);

            if (category == null)
            {
                return Fail("Category not found.");
            }

            category.IsDeleted = false;

            category.DeletedAt = null;

            _unitOfWork.Categories.Update(category);

            await _unitOfWork.SaveChangesAsync();

            return Success("Category restored successfully.");
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
    }
}
