using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuickServePOS.DbContextData.Data;
using QuickServePOS.Models.Configurations;
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
        private readonly JwtSettings _jwtSettings;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context, IJwtService jwtService, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _AppDbcontext = context;
            _jwtService = jwtService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var emaillower = dto.Email.ToLower();
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
            var profile = new UserProfileEntity
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

            var token = _jwtService.GenerateToken(user.Id, user.Email!, role!);

            var refreshToken = _jwtService.GenerateRefreshToken();

            var RefreshTokenEntity = new RefreshTokenEntity
            {
                RefreshToken = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInMinutes),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            await _AppDbcontext.RefreshTokens.AddAsync(RefreshTokenEntity);
            await _AppDbcontext.SaveChangesAsync();

            return new LoginApiResponseDto
            {
                Message = "Login successful",
                Email = user.Email,
                Role = role,
                AccessToken = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<LoginApiResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto refreshDto)
        {
            var existingToken=await  _AppDbcontext.RefreshTokens
                .Include(x=> x.User)
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshDto.RefreshToken);

            if (existingToken == null)
                return null;

            if(existingToken.IsRevoked)
                  return null;

            if(existingToken.ExpiryDate < DateTime.UtcNow)
                return null;

            var user = existingToken.User;

            var roles = await _userManager.GetRolesAsync(user);

            var role = roles.FirstOrDefault();

            // REVOKE OLD TOKEN

            existingToken.IsRevoked = true;

            // CREATE NEW TOKENS

            var newAccessToken= _jwtService.GenerateToken(user.Id, user.Email!, role!);

            var newRefreshToken = _jwtService.GenerateRefreshToken();

            var newrefreshTokenEntity = new RefreshTokenEntity
            {
                RefreshToken = newRefreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInMinutes),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            await _AppDbcontext.RefreshTokens.AddAsync(newrefreshTokenEntity);

            await _AppDbcontext.SaveChangesAsync();

            return new LoginApiResponseDto
            {
                Message = "Token refreshed successfully",
                Email = user.Email,
                Role = role,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
             };

        }
    }
}
