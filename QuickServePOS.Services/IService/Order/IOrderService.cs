using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.IService.Order
{
    public interface IOrderService
    {
        Task<ApiResponse> CreateAsync( OrderCreateDto dto);

        Task<ApiResponse> AddItemAsync(OrderItemCreateDto dto);

        Task<ApiDataResponse<OrderDetailsDto>> GetByIdAsync(int orderId);

        Task<ApiDataResponse<OrderDetailsDto>>GetRunningOrderByTableIdAsync(int tableId);

        Task<ApiResponse> CompleteOrderAsync(int orderId);

        Task<bool> UpdateCartItemAsync(UpdateCartItemDto dto);
    }
}
