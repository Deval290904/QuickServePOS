using Microsoft.EntityFrameworkCore;
using QuickServePOS.DbContextData.Data;
using QuickServePOS.Models.Entities.Enums;
using QuickServePOS.Models.Entities.Order;
using QuickServePOS.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _AppDbcontext;

        public OrderRepository(AppDbContext appDbContext)
        {
            _AppDbcontext = appDbContext;
        }

        public async Task<List<OrderEntity>> GetAllAsync()
        {
            return await _AppDbcontext.Orders.Include(x => x.Table).ToListAsync();
        }

        public async Task<OrderEntity?> GetByIdAsync(int id)
        {
            return await _AppDbcontext.Orders.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<OrderEntity?> GetOrderDetailsAsync(int orderId)
        {
            return await _AppDbcontext.Orders
                .Include(x => x.Table)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.MenuItem)
                .FirstOrDefaultAsync(x => x.Id == orderId);
        }

        public async Task<OrderEntity?> GetRunningOrderByTableAsync(int tableId)
        {
            return await _AppDbcontext.Orders
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(x =>
                    x.TableId == tableId &&
                    x.Status == OrderStatus.Running);
        }

        public async Task<bool> OrderNoExistsAsync(string orderNo)
        {
            return await _AppDbcontext.Orders
                .AnyAsync(x => x.OrderNo == orderNo);
        }

        public async Task AddAsync(OrderEntity entity)
        {
            await _AppDbcontext.Orders.AddAsync(entity);
        }

        public void Update(OrderEntity entity)
        {
            _AppDbcontext.Orders.Update(entity);
        }

        public void Delete(OrderEntity entity)
        {
            _AppDbcontext.Orders.Remove(entity);
        }
    }
}
