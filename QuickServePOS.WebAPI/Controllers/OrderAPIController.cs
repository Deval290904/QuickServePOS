using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Order;
using QuickServePOS.Services.IService.Order;


namespace QuickServePOS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderAPIController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
        {
            var response =
                await _orderService.CreateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        // ADD ITEM

        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItem([FromBody] OrderItemCreateDto dto)
        {
            var response =await _orderService.AddItemAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        // ORDER DETAILS

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _orderService.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        // RUNNING ORDER BY TABLE
      
        [HttpGet("RunningByTable/{tableId}")]
        public async Task<IActionResult>RunningByTable(int tableId)
        {
            var response =await _orderService.GetRunningOrderByTableIdAsync( tableId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("Cart/{orderId}")]
        public async Task<IActionResult> Cart(int orderId)
        {
            var response =await _orderService.GetByIdAsync(orderId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }


    }
}
