using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.IService.Menu
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();

        Task<CategoryDto?> GetByIdAsync(int id);

        Task<ApiResponse> CreateAsync(CreateCategoryDto dto);

        Task<ApiResponse> UpdateAsync(UpdateCategoryDto dto);

        Task<ApiResponse> DeleteAsync(int id);

        Task<List<CategoryDto>> GetDeletedAsync();

        Task<ApiResponse> RestoreAsync(int id);
    }
}
