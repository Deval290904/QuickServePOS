using QuickServePOS.Models.DTO.Admin;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.ViewModel;

namespace QuickServePOS.Services.IService
{
    public interface IAdminService
    {
        Task<ApiResponse> CreateStaffAsync(CreateStaffAccountDto model);

        Task<List<string>> GetStaffRolesAsync();

        Task<List<StaffListDto>> GetStaffListAsync();

        Task<ApiResponse> DeleteStaffAsync(string userId);

        Task<ApiResponse> RestoreStaffAsync(string userId);

        Task<List<StaffListDto>> GetDeletedStaffAsync();

        Task<ApiResponse> PermanentDeleteStaffAsync(string userId);

        Task<DashboardStatsDto> GetStaffStatsAsync();

        Task<UpdateStaffDto?> GetStaffByIdAsync(string userId);

        Task<ApiResponse> UpdateStaffAsync(UpdateStaffDto model);
    }
}
