using QuickServePOS.Models.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.IRepositories
{
    public interface IOrderRepository 
    {
        Task AddAsync(OrderEntity entity);

        void Update(OrderEntity entity);

        Task<OrderEntity?> GetByIdAsync(int id);

        Task<int> CountAsync();

        Task<OrderEntity?> GetRunningOrderByTableIdAsync(int tableId);

        Task<OrderEntity?> GetOrderWithItemsAsync(int orderId);

        Task<bool> ExistsRunningOrderAsync(int tableId);
    }
}
