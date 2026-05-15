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
        public async Task<ApiResponse> CreateAsync(
             OrderCreateDto dto)
        {
            // Check existing running order

            if (dto.TableId.HasValue)
            {
                var exists = await _unitOfWork.Orders.ExistsRunningOrderAsync(dto.TableId.Value);

                if (exists)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Running order already exists for this table."
                    };
                }
            }

            var entity = _mapper.Map<OrderEntity>(dto);

            entity.OrderNo =await GenerateOrderNoAsync();

            entity.Status = OrderStatus.Running;

            entity.Subtotal = 0;
            entity.TaxAmount = 0;
            entity.DiscountAmount = 0;
            entity.TotalAmount = 0;

            await _unitOfWork.Orders.AddAsync(entity);

            // Update table status

            if (dto.TableId.HasValue)
            {
                var table = await _unitOfWork.Tables
                    .GetByIdAsync(dto.TableId.Value);

                if (table != null)
                {
                    table.Status = TableStatus.Occupied;

                    _unitOfWork.Tables.Update(table);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true,
                Message = "Order created successfully."
            };
        }

        public async Task<ApiResponse> AddItemAsync(
      OrderItemCreateDto dto)
        {
            // =========================
            // GET ORDER WITH ITEMS
            // =========================

            var order = await _unitOfWork.Orders
                .GetOrderWithItemsAsync(dto.OrderId);

            if (order == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Order not found."
                };
            }

            // =========================
            // GET MENU ITEM
            // =========================

            var menuItem = await _unitOfWork.MenuItems
                .GetByIdAsync(dto.MenuItemId);

            if (menuItem == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Menu item not found."
                };
            }

            // =========================
            // CHECK EXISTING ITEM
            // =========================

            var existingItem = order.OrderItems
                .FirstOrDefault(x =>
                    x.MenuItemId == dto.MenuItemId &&
                    !x.IsDeleted);

            // =========================
            // UPDATE EXISTING ITEM
            // =========================

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;

                existingItem.TotalPrice =
                    existingItem.Quantity *
                    existingItem.UnitPrice;

                existingItem.SpecialInstruction =
                    dto.SpecialInstruction;
            }

            // =========================
            // ADD NEW ITEM
            // =========================

            else
            {
                var item = new OrderItemEntity
                {
                    OrderId = dto.OrderId,

                    MenuItemId = dto.MenuItemId,

                    Quantity = dto.Quantity,

                    UnitPrice = menuItem.FullPrice,

                    TotalPrice =
                        dto.Quantity *
                        menuItem.FullPrice,

                    SpecialInstruction =
                        dto.SpecialInstruction
                };

                order.OrderItems.Add(item);
            }

            // =========================
            // RECALCULATE TOTALS
            // =========================

            order.Subtotal = order.OrderItems
                .Where(x => !x.IsDeleted)
                .Sum(x => x.TotalPrice);

            order.TotalAmount =
                order.Subtotal +
                order.TaxAmount -
                order.DiscountAmount;

            // =========================
            // UPDATE ORDER
            // =========================

            _unitOfWork.Orders.Update(order);

            // =========================
            // SAVE CHANGES
            // =========================

            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true,
                Message = "Item added successfully."
            };
        }

        public async Task<ApiDataResponse<OrderDetailsDto>>GetByIdAsync(int orderId)
        {
            var entity = await _unitOfWork
                .Orders
                .GetOrderWithItemsAsync(orderId);

            if (entity == null)
            {
                return new ApiDataResponse<OrderDetailsDto>
                {
                    Success = false,
                    Message = "Order not found."
                };
            }

            var data = _mapper.Map<OrderDetailsDto>(entity);

            return new ApiDataResponse<OrderDetailsDto>
            {
                Success = true,
                Data = data
            };
        }

        public async Task<ApiDataResponse<OrderDetailsDto>>GetRunningOrderByTableIdAsync(
                int tableId)
        {
            var entity = await _unitOfWork
                .Orders
                .GetRunningOrderByTableIdAsync(tableId);

            if (entity == null)
            {
                return new ApiDataResponse<OrderDetailsDto>
                {
                    Success = false,
                    Message = "Running order not found."
                };
            }

            var data = _mapper.Map<OrderDetailsDto>(entity);

            return new ApiDataResponse<OrderDetailsDto>
            {
                Success = true,
                Data = data
            };
        }

        // =========================
        // PRIVATE METHODS
        // =========================

        private async Task<string>
            GenerateOrderNoAsync()
        {
            var count =
                await _unitOfWork.Orders.CountAsync();

            return
                $"ORD-{DateTime.UtcNow:yyyyMMdd}-{(count + 1):D4}";
        }

        public async Task<ApiResponse> CompleteOrderAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);

            if (order == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Order not found."
                };
            }

            order.Status = OrderStatus.Completed;

            _unitOfWork.Orders.Update(order);

            // Free table

            if (order.TableId.HasValue)
            {
                var table = await _unitOfWork.Tables
                    .GetByIdAsync(order.TableId.Value);

                if (table != null)
                {
                    table.Status = TableStatus.Available;

                    _unitOfWork.Tables.Update(table);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true,
                Message = "Order completed successfully."
            };
        }

        public async Task<bool> UpdateCartItemAsync(UpdateCartItemDto dto)
        {
            var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(dto.OrderItemId);

            if (orderItem == null)
            {
                return false;
            }

            orderItem.Quantity = dto.Quantity;

            if (dto.SpecialInstruction != null)
            {
                orderItem.SpecialInstruction =
                    dto.SpecialInstruction;
            }
            orderItem.TotalPrice =
                orderItem.Quantity *
                orderItem.UnitPrice;
            _unitOfWork.OrderItems.Update(orderItem);

            // RECALCULATE ORDER

            var order = await _unitOfWork.Orders.GetOrderWithItemsAsync(orderItem.OrderId);

            if (order == null)
            {
                return false;
            }

            RecalculateOrderTotals(order);

            _unitOfWork.Orders.Update(order);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCartItemAsync(int orderItemId)
        {
            var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(orderItemId);

            if (orderItem == null)
            {
                return false;
            }

            var order = await _unitOfWork
                .Orders.GetOrderWithItemsAsync(orderItem.OrderId);

            if (order == null)
            {
                return false;
            }

            _unitOfWork.OrderItems.Remove(orderItem);

            // REMOVE FROM MEMORY ALSO

            order.OrderItems.Remove(orderItem);

            RecalculateOrderTotals(order);

            _unitOfWork.Orders.Update(order);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private void RecalculateOrderTotals(OrderEntity order)
        {
            order.Subtotal =
                order.OrderItems
                .Sum(x => x.TotalPrice);

            // GST calculation later

            order.TotalAmount =
                order.Subtotal
                + order.TaxAmount
                - order.DiscountAmount;
        }

    }
}

