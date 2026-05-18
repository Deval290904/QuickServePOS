using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.ViewModel.Admin;
using QuickServePOS.Services.IService.Admin;


namespace QuickServePOS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardAPIController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        private readonly IMapper _mapper;

        public DashboardAPIController(IDashboardService dashboardService,IMapper mapper)
        {
            _dashboardService = dashboardService;
            _mapper = mapper;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var dto = await _dashboardService
                .GetDashboardDataAsync();

            

            return Ok(dto);
        }
    }
}
