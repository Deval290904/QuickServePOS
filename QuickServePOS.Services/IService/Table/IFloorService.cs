using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Floor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.IService.Table
{
    public interface IFloorService
    {
        Task<List<FloorListDto>> GetAllAsync();

        Task<FloorUpdateDto?> GetByIdAsync(int id);

        Task<ApiResponse> CreateAsync(FloorCreateDto dto);

        Task<ApiResponse> UpdateAsync(FloorUpdateDto dto);

        Task<ApiResponse> DeleteAsync(int id);

        Task<ApiResponse> RestoreAsync(int id);

        Task<List<FloorListDto>> GetDeletedAsync();
    }
}
