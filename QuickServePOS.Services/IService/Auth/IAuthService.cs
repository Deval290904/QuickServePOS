using QuickServePOS.Models.DTO.Auth;
using QuickServePOS.Models.DTO.Common;

namespace QuickServePOS.Services.IService.Auth
{
    public interface IAuthService
    {
        Task<ApiResponse> RegisterAsync(RegisterDto dto);
        Task<LoginApiResponseDto> LoginAsync(LoginDto dto);
        Task<LoginApiResponseDto> RefreshTokenAsync(RefreshTokenRequestDto refreshDto);
        Task<ApiResponse> ConfirmEmailAsync(string userId, string token);
        Task<ApiResponse> ForgotPasswordAsync(ForgotPasswordDto dto);

        Task<ApiResponse> ResetPasswordAsync(ResetPasswordDto dto);
    }
}
