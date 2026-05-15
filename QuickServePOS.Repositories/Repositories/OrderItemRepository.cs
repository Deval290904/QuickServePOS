using Microsoft.EntityFrameworkCore;
using QuickServePOS.DbContextData.Data;
using QuickServePOS.Models.Entities.Order;
using QuickServePOS.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _AppDbcontext;

        public OrderItemRepository(AppDbContext appDbContext)
        {
            _AppDbcontext = appDbContext;
        }

        public async Task<OrderItemEntity?> GetByIdAsync(int id)
        {
            return await _AppDbcontext.OrderItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(OrderItemEntity entity)
        {
            await _AppDbcontext.OrderItems
                .AddAsync(entity);
        }

        public void Update(
            OrderItemEntity entity)        {
            _AppDbcontext.OrderItems.Update(entity);
        }

        public void Remove(
            OrderItemEntity entity)
        {
            _AppDbcontext.OrderItems.Remove(entity);
        }
    }
}
