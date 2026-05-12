using Microsoft.AspNetCore.Http;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.IService.Common
{
    public interface IProfileService
    {
        Task<ProfileDto?> GetProfileAsync(string userId);

        Task<ApiResponse> UpdateProfileAsync(string userId,UpdateProfileDto model);

        Task<ApiResponse> UploadProfileImageAsync(string userId,IFormFile image);

    }
}
