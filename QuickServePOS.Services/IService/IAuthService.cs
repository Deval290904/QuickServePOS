using QuickServePOS.Models.DTO.Auth;

namespace QuickServePOS.Services.IService
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<LoginApiResponseDto> LoginAsync(LoginDto dto);
        Task<LoginApiResponseDto> RefreshTokenAsync(RefreshTokenRequestDto refreshDto);
    }
}
