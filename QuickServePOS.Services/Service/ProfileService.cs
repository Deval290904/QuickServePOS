using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using QuickServePOS.DbContextData.Data;
using QuickServePOS.DbContextData.Migrations;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Profile;
using QuickServePOS.Models.Entities;
using QuickServePOS.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.Service
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _AppDbContext;

        public ProfileService(UserManager<ApplicationUser> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _AppDbContext = appDbContext;
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
                user.UserProfile = new UserProfile
                {
                    UserId = user.Id
                };
            }

            // Update UserProfile
            user.UserProfile.DOB = model.DOB;
            user.UserProfile.Gender = model.Gender;
            user.UserProfile.Address = model.Address;

            await _userManager.UpdateAsync(user);

            await _AppDbContext.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true,
                Message = "Profile updated successfully"
            };
        }
    }
}
