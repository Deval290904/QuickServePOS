using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuickServePOS.DbContextData.Data;
using QuickServePOS.Models.Configurations;
using QuickServePOS.Models.DTO.Auth;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.Entities;
using QuickServePOS.Services.IService.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuickServePOS.Services.Service.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _AppDbcontext;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;
        private readonly IEmailService _emailService;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context, IJwtService jwtService, IOptions<JwtSettings> jwtSettings, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _AppDbcontext = context;
            _jwtService = jwtService;
            _jwtSettings = jwtSettings.Value;
            _emailService = emailService;
        }

        public async Task<ApiResponse> RegisterAsync(RegisterDto dto)
        {
            var emailLower = dto.Email.ToLower();

            // CHECK EMAIL EXISTS

            var existingUser = await _userManager
                .FindByEmailAsync(emailLower);

            if (existingUser != null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Email already exists."
                };
            }

            var user = new ApplicationUser
            {
                UserName = emailLower,
                Email = emailLower,
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager
                .CreateAsync(user, dto.Password);

            // CREATE FAILED

            if (!result.Succeeded)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = string.Join(", ",result.Errors.Select(x => x.Description))
                };
            }

            // DEFAULT ROLE

            await _userManager.AddToRoleAsync( user,"Customer");

            // GENERATE EMAIL TOKEN

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedToken =Uri.EscapeDataString(token);

            var confirmationLink =
                $"https://localhost:7290/api/AuthenticationAPI/confirm-email?userId={user.Id}&token={encodedToken}";

            var body = GenerateConfirmationEmailDesign(
                    user.Name,
                    confirmationLink);


            await _emailService.SendEmailAsync(
                user.Email!,
                "Confirm Your Email",
                body);

            return new ApiResponse
            {
                Success = true,
                Message = "Registration successful. Please confirm your email."
            };
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

            if (!user.EmailConfirmed)
                return new LoginApiResponseDto
                {
                    Message = "Please confirm your email first",
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
                ExpiryDate = dto.RememberMe
                                ? DateTime.UtcNow.AddDays(7)
                                : DateTime.UtcNow.AddHours(1),
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

        public async Task<ApiResponse> ConfirmEmailAsync( string userId,string token)
        {
            var user = await _userManager .FindByIdAsync(userId);

            if (user == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Invalid user."
                };
            }

            if (user.EmailConfirmed)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Email already confirmed."
                };
            }

            var result = await _userManager .ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Invalid or expired token."
                };
            }

            // CREATE USER PROFILE AFTER CONFIRMATION

            var existingProfile = await _AppDbcontext
                .UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (existingProfile == null)
            {
                var profile = new UserProfileEntity
                {
                    UserId = user.Id,
                    JoiningDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

                await _AppDbcontext.UserProfiles
                    .AddAsync(profile);

                await _AppDbcontext.SaveChangesAsync();
            }

            return new ApiResponse
            {
                Success = true,
                Message = "Email confirmed successfully."
            };
        }

        private string GenerateConfirmationEmailDesign(string name,string confirmationLink)
        {
                   return $@"
                    <div style='
                        font-family:Arial,sans-serif;
                        max-width:500px;
                        margin:auto;
                        padding:20px;
                        border:1px solid #ddd;
                        border-radius:8px;'>

                        <h2 style='color:#f7681b;'>
                            QuickServe POS
                        </h2>

                        <p>Hello <b>{name}</b>,</p>

                        <p>
                            Thank you for registering with
                            QuickServe POS.
                        </p>

                        <p>
                            Please confirm your email address
                            by clicking the button below.
                        </p>

                        <div style='margin:30px 0;'>

                            <a href='{confirmationLink}'
                               style='
                                background:#f7681b;
                                color:white;
                                padding:12px 20px;
                                text-decoration:none;
                                border-radius:5px;'>

                                Confirm Email

                            </a>

                        </div>

                        <p style='font-size:14px;color:gray;'>
                            This link will expire in 24 hours.
                        </p>

                        <p style='font-size:14px;color:gray;'>
                            If you did not create this account,
                            please ignore this email.
                        </p>

                        <hr />

                        <p style='font-size:12px;color:gray;'>
                            © 2026 QuickServe POS
                        </p>

                    </div>";
        }

        public async Task<ApiResponse> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var emailLower = dto.Email.ToLower();

            var user = await _userManager.FindByEmailAsync(emailLower);

            if (user == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message =  "Email not found"
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken =Uri.EscapeDataString(token);

            var resetLink =$"https://localhost:7237/Authentication/ResetPassword?email={user.Email}&token={encodedToken}";

            var body = GenerateForgotPasswordEmailDesign(user.Name,resetLink);

            await _emailService.SendEmailAsync(user.Email!,
                "Reset Password",
                body);

            return new ApiResponse
            {
                Success = true,
                Message = "Password reset link sent successfully"
            };
        }

        public async Task<ApiResponse> ResetPasswordAsync(ResetPasswordDto dto)
        {
           
            var user = await _userManager.FindByEmailAsync(dto.Email.ToLower());

            if (user == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Invalid request."
                };
            }
            // DECODE TOKEN
            var decodedToken =Uri.UnescapeDataString(dto.Token);

            var result = await _userManager.ResetPasswordAsync(
                    user,
                    decodedToken,
                    dto.NewPassword
            );

            if (!result.Succeeded)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Invalid or expired token."
                };
            }

            return new ApiResponse
            {
                Success = true,
                Message = "Password reset successful."
            };
        }

        private string GenerateForgotPasswordEmailDesign(string name,string resetLink)
        {
            return $@"
            <div style='
                font-family:Arial,sans-serif;
                max-width:500px;
                margin:auto;
                padding:20px;
                border:1px solid #ddd;
                border-radius:8px;'>

                <h2 style='color:#f7681b;'>
                    QuickServe POS
                </h2>

                <p>Hello <b>{name}</b>,</p>

                <p>
                    We received a request to reset
                    your password.
                </p>

                <div style='margin:30px 0;'>

                    <a href='{resetLink}'
                       style='
                        background:#f7681b;
                        color:white;
                        padding:12px 20px;
                        text-decoration:none;
                        border-radius:5px;'>

                        Reset Password

                    </a>

                </div>

                <p style='font-size:14px;color:gray;'>
                    This link expires in 30 minutes.
                </p>

                <p style='font-size:14px;color:gray;'>
                    If you did not request this,
                    please ignore this email.
                </p>

            </div>";
        }
    }
}
