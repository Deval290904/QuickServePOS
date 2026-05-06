using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Auth;
using QuickServePOS.Services.IService;

namespace QuickServePOS.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationAPIController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthenticationAPIController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _service.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var response = await _service.LoginAsync(dto);

            if (response.AccessToken == null)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("Refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto refreshDto)
        {
            var result= await _service.RefreshTokenAsync(refreshDto);

            if(result==null)
            {
                return Unauthorized(new
                {
                        Message = "Invalid refresh token."
                });
            }
            return Ok(result);
        }
    }
}
