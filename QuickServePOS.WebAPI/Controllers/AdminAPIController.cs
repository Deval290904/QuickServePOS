using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Admin;
using QuickServePOS.Services.IService;

namespace QuickServePOS.WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAPIController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminAPIController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("Create-Staff")]
        public async Task<IActionResult> CreateStaff(CreateStaffAccountDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _adminService.CreateStaffAsync(model);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpGet("RolesList")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _adminService.GetStaffRolesAsync();
            return Ok(roles);
        }


        [HttpGet("StaffList")]
        public async Task<IActionResult> GetStaffList()
        {
            var staffList = await _adminService.GetStaffListAsync();
            return Ok(staffList);
        }

        [HttpGet("Get-Staff/{id}")]
        public async Task<IActionResult> GetStaff(string id)
        {
            var result = await _adminService.GetStaffByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("Update-Staff")]
        public async Task<IActionResult> UpdateStaff(UpdateStaffDto model)
        {
            var result = await _adminService.UpdateStaffAsync(model);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("Delete-Staff/{id}")]
        public async Task<IActionResult> DeleteStaff(string id)
        {
            var result = await _adminService.DeleteStaffAsync(id);
            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, message = result.Message });
        }

        [HttpPut("Restore-Staff/{id}")]
        public async Task<IActionResult> RestoreStaff(string id)
        {
            var result = await _adminService.RestoreStaffAsync(id);

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });
            return Ok(new { success = true, message = result.Message });
        }

        [HttpGet("Deleted-Staff-List")]
        public async Task<IActionResult> GetDeletedStaff()
        {
            var data = await _adminService.GetDeletedStaffAsync();

            return Ok(data);
        }

        [HttpDelete("Delete-Permanent/{id}")]
        public async Task<IActionResult> DeletePermanentStaff(string id)
        {
            var result = await _adminService.PermanentDeleteStaffAsync(id);

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, message = result.Message });
        }

        [HttpGet("Staff-Stats")]
        public async Task<IActionResult> GetStaffStats()
        {
            var result = await _adminService.GetStaffStatsAsync();

            return Ok(result);
        }
    }
}
