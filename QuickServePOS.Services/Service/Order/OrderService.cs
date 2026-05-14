using AutoMapper;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Order;
using QuickServePOS.Models.Entities.Enums;
using QuickServePOS.Models.Entities.Order;
using QuickServePOS.Repositories.IUnitofWork;
using QuickServePOS.Services.IService.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.Service.Order
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse> CreateAsync(OrderCreateDto dto)
        {
            // Check table required

            if (dto.OrderType == OrderType.DineIn && dto.TableId == null)
            {
                return new ApiResponse { Success = false, Message = "Table is required." };
            }
            // Running Order Check
            if (dto.TableId.HasValue)
            {
                var existingOrder = await _unitOfWork.Orders.GetRunningOrderByTableAsync(dto.TableId.Value);

                if (existingOrder != null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = $"Table already has running order: {existingOrder.OrderNo}"
                    };
                }
            }
            // Generate order number

            var orderNo = await GenerateOrderNoAsync();

            // Create entity

            var entity = _mapper.Map<OrderEntity>(dto);

            entity.OrderNo = orderNo;

            entity.Status = OrderStatus.Running;

            entity.Subtotal = 0;

            entity.TaxAmount = 0;

            entity.DiscountAmount = 0;

            entity.TotalAmount = 0;

            // Save

            await _unitOfWork.Orders.AddAsync(entity);

            await _unitOfWork.SaveChangesAsync();

            // Update table status

            if (entity.TableId.HasValue)
            {
                var table = await _unitOfWork.Tables.GetByIdAsync(entity.TableId.Value);

                if (table != null)
                {
                    table.Status = TableStatus.Occupied;

                    _unitOfWork.Tables.Update(table);

                    await _unitOfWork.SaveChangesAsync();
                }
            }

            return new ApiResponse { Success = true, Message = "Order created successfully." };
        }

        public async Task<ApiResponse> AddItemAsync(OrderItemCreateDto dto)
        {
            var order = await _unitOfWork.Orders.GetOrderDetailsAsync(dto.OrderId);

            if (order == null)
            {
                return new ApiResponse{Success = false, Message = "Order not found."};
            }

            var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(dto.MenuItemId);

            if (menuItem == null)
            {
                return new ApiResponse{ Success = false, Message = "Menu item not found."};
            }

            var item = new OrderItemEntity
            {
                OrderId = dto.OrderId,

                MenuItemId = dto.MenuItemId,

                Quantity = dto.Quantity,

                UnitPrice = menuItem.FullPrice,

                TotalPrice = menuItem.FullPrice
                    * dto.Quantity,

                SpecialInstruction =
                    dto.SpecialInstruction
            };

            order.OrderItems.Add(item);

            // Recalculate totals

            order.Subtotal =order.OrderItems.Sum(x => x.TotalPrice);

            order.TotalAmount =
                order.Subtotal
                + order.TaxAmount
                - order.DiscountAmount;

            _unitOfWork.Orders.Update(order);

            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse{Success = true,Message = "Item added successfully."};
        }

        public async Task<ApiDataResponse<OrderDetailsDto>>GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Orders.GetOrderDetailsAsync(id);

            if (entity == null)
            {
                return new ApiDataResponse<OrderDetailsDto>
                {
                    Success = false,
                    Message = "Order not found."
                };
            }

            var data = _mapper.Map<OrderDetailsDto>(
                entity);

            return new ApiDataResponse<OrderDetailsDto>
            {
                Success = true,
                Data = data
            };
        }

        public async Task<ApiDataResponse<List<OrderListDto>>>GetAllAsync()
        {
            var entities = await _unitOfWork.Orders.GetAllAsync();

            var data = _mapper.Map<List<OrderListDto>>(entities);

            return new ApiDataResponse<List<OrderListDto>>
            {
                Success = true,
                Message="Orders retrieved successfully.",
                Data = data
            };
        }

        public async Task<ApiResponse> UpdateAsync(OrderUpdateDto dto)
        {
            var entity = await _unitOfWork.Orders.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Order not found."
                };
            }

            entity.Status = dto.Status;

            entity.Notes = dto.Notes;

            _unitOfWork.Orders.Update(entity);

            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true,
                Message = "Order updated successfully."
            };
        }

        public async Task<ApiDataResponse<OrderDetailsDto>>GetRunningOrderByTableAsync(int tableId)
        {
            var entity = await _unitOfWork.Orders.GetRunningOrderByTableAsync(tableId);

            if (entity == null)
            {
                return new ApiDataResponse<OrderDetailsDto>
                {
                    Success = false,
                    Message = "No running order found."
                };
            }

            var data = _mapper.Map<OrderDetailsDto>(entity);

            return new ApiDataResponse<OrderDetailsDto>
            {
                Success = true,
                Data = data
            };
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Orders.GetByIdAsync(id);

            if (entity == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Order not found."
                };
            }

            entity.IsDeleted = true;

            _unitOfWork.Orders.Update(entity);

            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true,
                Message = "Order deleted successfully."
            };
        }
        private async Task<string> GenerateOrderNoAsync()
        {
            var count = (await _unitOfWork.Orders
                .GetAllAsync()).Count + 1;

            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{count:D4}";
        }

    }
}

