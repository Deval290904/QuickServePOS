using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Profile;
using QuickServePOS.Services.IService;
using System.Security.Claims;

namespace QuickServePOS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileAPIController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileAPIController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _profileService.GetProfileAsync(userId);

            if (result == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Profile not found"
                });
            }

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        [HttpPost("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _profileService
                .UpdateProfileAsync(userId, model);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
