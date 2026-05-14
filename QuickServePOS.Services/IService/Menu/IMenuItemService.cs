using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Menu;
using QuickServePOS.Models.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.IService.Menu
{
    public interface IMenuItemService
    {
        Task<List<MenuItemDto>> GetAllAsync();

        Task<MenuItemDto?> GetByIdAsync(int id);

        Task<ApiResponse> CreateAsync(CreateMenuItemDto dto);

        Task<ApiResponse> UpdateAsync(UpdateMenuItemDto dto);

        Task<ApiResponse> DeleteAsync(int id);

        Task<List<MenuItemDto>> GetDeletedAsync();

        Task<ApiResponse> RestoreAsync(int id);

        Task<ApiDataResponse<List<MenuItemDto>>> GetByCategoryAsync(int categoryId);
    }
}
