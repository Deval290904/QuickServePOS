using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using QuickServePOS.DbContextData.Data;
using QuickServePOS.Models.DTO.Auth;
using QuickServePOS.Models.Entities;
using QuickServePOS.Services.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuickServePOS.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _AppDbcontext;
        private readonly IJwtService _jwtService;

        public AuthService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, AppDbContext context, IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _AppDbcontext = context;
            _jwtService = jwtService;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
             var emaillower= dto.Email.ToLower();
            var user = new ApplicationUser
            {
                UserName = emaillower,
                Email = emaillower,
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return string.Join(", ", result.Errors.Select(x => x.Description));

            // Default role
            await _userManager.AddToRoleAsync(user, "Customer");

            // Create UserProfile
            var profile = new UserProfile
            {
                UserId = user.Id,
                JoiningDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            await _AppDbcontext.UserProfiles.AddAsync(profile);
            await _AppDbcontext.SaveChangesAsync();
            return "Registered Successfully";
        }

        public async Task<LoginApiResponseDto> LoginAsync(LoginDto dto)
        {
            var emaillower = dto.Email.ToLower();

            var user = await _userManager.FindByEmailAsync(emaillower);

            if (user == null)
                return new LoginApiResponseDto
                {
                    Message = "User not found",
                    Email = null,
                    Role = null,
                    AccessToken = null
                };

            var result = await _signInManager.CheckPasswordSignInAsync(
                user, dto.Password, lockoutOnFailure: true);

            if (result.IsLockedOut)
                return new LoginApiResponseDto
                {
                    Message = "Account locked. Try later",
                    Email = null,
                    Role = null,
                    AccessToken = null
                };

            if (!result.Succeeded)
                return new LoginApiResponseDto
                {
                    Message = "Invalid credentials",
                    Email = null,
                    Role = null,
                    AccessToken = null
                };

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            // GENERATE JWT TOKEN USING JWT SERVICE
          
            var token =_jwtService.GenerateToken(user.Id,user.Email!, role!);
            

            return new LoginApiResponseDto
            {
                Message = "Login successful",
                Email = user.Email,
                Role = role,
                AccessToken = token
            };
        }

    }
}
