using QuickServePOS.Models.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.IRepositories
{
    public interface IOrderItemRepository
    {
        Task<OrderItemEntity?> GetByIdAsync(int id);

        void Update(OrderItemEntity entity);

        Task AddAsync(OrderItemEntity entity);

        void Remove(OrderItemEntity entity);
    
    }
}
