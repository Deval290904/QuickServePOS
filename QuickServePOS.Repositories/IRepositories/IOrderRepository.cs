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
        Task<OrderEntity?> GetByIdAsync(int id);

        Task<List<OrderEntity>> GetAllAsync();

        Task<OrderEntity?> GetOrderDetailsAsync(int orderId);

        Task<OrderEntity?> GetRunningOrderByTableAsync(int tableId);

        Task<bool> OrderNoExistsAsync(string orderNo);

        Task AddAsync(OrderEntity entity);

        void Update(OrderEntity entity);

        void Delete(OrderEntity entity);
    }
}
