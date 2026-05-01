using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IConfiguration _config;

        public AuthService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
             var emaillower= dto.Email.ToLower();
            var user = new ApplicationUser
            {
                UserName = emaillower,
                Email = emaillower,
                Name = dto.Name
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return string.Join(", ", result.Errors.Select(x => x.Description));

            // Default role
            await _userManager.AddToRoleAsync(user, "Customer");

            return "Registered Successfully";
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var emaillower = dto.Email.ToLower();

            var user = await _userManager.FindByEmailAsync(emaillower);

            if (user == null)
                return new AuthResponseDto
                {
                    Message = "Invalid credentials",
                    AccessToken = null
                }; 

            var result = await _signInManager.CheckPasswordSignInAsync(
                user, dto.Password, lockoutOnFailure: true);

            if (result.IsLockedOut)
                return new AuthResponseDto
                {
                    Message = "Account locked. Try later",
                    AccessToken = null
                };

            if (!result.Succeeded)
                return new AuthResponseDto
                {
                    Message = "Invalid credentials",
                    AccessToken = null
                };

            var token = await GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Message = "Login successful",
                AccessToken = token
            };
        }


        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("UserId", user.Id)
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
