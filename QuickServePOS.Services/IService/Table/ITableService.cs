using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.RestaurantTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.IService.Table
{
    public interface ITableService
    {
        Task<List<TableListDto>> GetAllAsync();

        Task<TableUpdateDto?> GetByIdAsync(int id);

        Task<ApiResponse> CreateAsync(TableCreateDto dto);

        Task<ApiResponse> UpdateAsync(TableUpdateDto dto);

        Task<ApiResponse> DeleteAsync(int id);

        Task<ApiResponse> RestoreAsync(int id);

        Task<List<TableListDto>> GetDeletedAsync();
    }
}
