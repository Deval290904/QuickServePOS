using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuickServePOS.DbContextData.Data;
using QuickServePOS.Models.DTO.Admin;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.Entities;
using QuickServePOS.Models.ViewModel;
using QuickServePOS.Services.IService;

public class AdminService : IAdminService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AppDbContext _AppDbcontext;

    public AdminService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        AppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _AppDbcontext = context;
    }

    public async Task<ApiResponse> CreateStaffAsync(CreateStaffAccountDto model)
    {
        var emaillower = model.Email.ToLower();
        // ✅ Check role exists in DB
        var roleExists = await _roleManager.RoleExistsAsync(model.Role);

        if (!roleExists)
        {
            return new ApiResponse
            {
                Success = false,
                Message = "Role does not exist in system"
            };
        }

        // ❌ Prevent creating customer/admin from here (business rule)
        var restrictedRoles = new[] { "Admin", "Customer" };

        if (restrictedRoles.Contains(model.Role))
        {
            return new ApiResponse
            {
                Success = false,
                Message = "This role cannot be created from admin panel"
            };
        }

        // ✅ Check email already exists
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = "User already exists with this email"
            };
        }

        // ✅ Create user
        var user = new ApplicationUser
        {
            UserName = emaillower,
            Email = emaillower,
            Name = model.Name,
            PhoneNumber = model.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return new ApiResponse
            {
                Success = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }

        // ✅ Assign role
        await _userManager.AddToRoleAsync(user, model.Role);

        // Create UserProfile
        var profile = new UserProfile
        {
            UserId = user.Id,
            JoiningDate = DateTime.UtcNow
        };

        await _AppDbcontext.UserProfiles.AddAsync(profile);

        await _AppDbcontext.SaveChangesAsync();

        return new ApiResponse
        {
            Success = true,
            Message = "Staff account created successfully"
        };
    }

    public async Task<List<string>> GetStaffRolesAsync()
    {
        // ❌ Exclude roles not allowed in UI
        var excludedRoles = new[] { "Admin", "Customer" };

        var roles = _roleManager.Roles
            .Where(r => !excludedRoles.Contains(r.Name))
            .Select(r => r.Name)
            .ToList();

        return await Task.FromResult(roles);
    }


    public async Task<List<StaffListDto>> GetStaffListAsync()
    {
        var users = _userManager.Users.ToList(); 

        var staffList = new List<StaffListDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var role = roles.FirstOrDefault();

            // ❌ Skip Admin & Customer
            if (role == "Admin" || role == "Customer")
                continue;

            staffList.Add(new StaffListDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Role = role
            });
        }

        return staffList;
    }

    public async Task<ApiResponse> DeleteStaffAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new ApiResponse { Success = false, Message = "User not found" };

        if (user.IsDeleted)
            return new ApiResponse { Success = false, Message = "User already deleted" };

        user.IsDeleted = true;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return new ApiResponse { Success = false, Message = "Delete failed" };

        return new ApiResponse { Success = true, Message = "Staff deleted successfully" };
    }

    public async Task<ApiResponse> RestoreStaffAsync(string userId)
    {
        var user = await _userManager.Users.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            return new ApiResponse { Success = false, Message = "User not found" };

        if (!user.IsDeleted)
            return new ApiResponse { Success = false, Message = "User is not deleted" };

        user.IsDeleted = false;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return new ApiResponse { Success = false, Message = "Restore failed" };

        return new ApiResponse { Success = true, Message = "Staff restored successfully" };
    }

    public async Task<List<StaffListDto>> GetDeletedStaffAsync()
    {
        var users = _userManager.Users.IgnoreQueryFilters().Where(x => x.IsDeleted).ToList();

        var staffList = new List<StaffListDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var role = roles.FirstOrDefault();

          
            if (role == "Admin" || role == "Customer")
                continue;

            staffList.Add(new StaffListDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Role = role
            });
        }
        return staffList;
    }

    public async Task<ApiResponse> PermanentDeleteStaffAsync(string userId)
    {
        var user = await _userManager.Users.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            return new ApiResponse { Success = false, Message = "User not found" };

        if (!user.IsDeleted)
            return new ApiResponse { Success = false, Message = "User must be deleted first" };

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
            return new ApiResponse { Success = false, Message = "Permanent delete failed" };

        return new ApiResponse { Success = true, Message = "Staff permanently deleted" };
    }


}