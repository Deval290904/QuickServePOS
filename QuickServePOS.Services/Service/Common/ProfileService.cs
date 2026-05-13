using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuickServePOS.DbContextData.Data;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Profile;
using QuickServePOS.Models.Entities;
using QuickServePOS.Models.Entities.Auth;
using QuickServePOS.Services.IService.Common;


namespace QuickServePOS.Services.Service.Common
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _AppDbContext;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _env;

        public ProfileService(UserManager<ApplicationUser> userManager, AppDbContext appDbContext, IImageService imageService, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _AppDbContext = appDbContext;
            _imageService = imageService;
            _env = env;
        }

        public async Task<ProfileDto?> GetProfileAsync(string userId)
        {
            var user = await _AppDbContext.Users.Include(x => x.UserProfile).FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return null;
            }

            return new ProfileDto
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,

                DOB = user.UserProfile?.DOB,
                Gender = user.UserProfile?.Gender,
                Address = user.UserProfile?.Address,
                JoiningDate = user.UserProfile?.JoiningDate,
                ProfileImagePath = user.UserProfile?.ProfileImagePath
            };
        }

        public async Task<ApiResponse> UpdateProfileAsync(string userId, UpdateProfileDto model)
        {
            var user = await _AppDbContext.Users.Include(x => x.UserProfile).FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return new ApiResponse { Success = false, Message = "User not found" };
            }
            user.Name = model.Name;
            user.PhoneNumber = model.PhoneNumber;

            // Create profile if not exists
            if (user.UserProfile == null)
            {
                user.UserProfile = new UserProfileEntity
                {
                    UserId = user.Id
                };
            }

            // Update UserProfile
            user.UserProfile.DOB = model.DOB;
            user.UserProfile.Gender = model.Gender;
            user.UserProfile.Address = model.Address;
            user.UserProfile.UpdatedAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            await _AppDbContext.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true,
                Message = "Profile updated successfully"
            };
        }

        public async Task<ApiResponse> UploadProfileImageAsync(string userId,IFormFile image)
        {
            var user = await _AppDbContext.Users
                .Include(x => x.UserProfile)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            // Create profile if null
            if (user.UserProfile == null)
            {
                user.UserProfile = new UserProfileEntity
                {
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow
                };
            }

            // Delete old image if exists
            if (!string.IsNullOrEmpty(
                user.UserProfile.ProfileImagePath))
            {
                var oldPath = Path.Combine(
                    _env.WebRootPath,
                    user.UserProfile.ProfileImagePath
                        .TrimStart('/'));

                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }
            }

            // Upload new image
            var imagePath = await _imageService
                .UploadImageAsync(
                    image,
                    "profile");

            user.UserProfile.ProfileImagePath =
                imagePath;

            user.UserProfile.UpdatedAt = DateTime.UtcNow;

            await _AppDbContext.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true,
                Message = "Profile image uploaded successfully"
            };
        }
    }
}
