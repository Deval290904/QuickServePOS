using Microsoft.EntityFrameworkCore;
using QuickServePOS.DbContextData.Data;
using QuickServePOS.Models.Entities.Enums;
using QuickServePOS.Models.Entities.KOT;
using QuickServePOS.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.Repositories
{
    public class KOTRepository : IKOTRepository
    {
        private readonly AppDbContext _AppDbContext;

        public KOTRepository(AppDbContext context)
        {
            _AppDbContext = context;
        }

        public async Task AddKOTAsync(KOTEntity kot)
        {
            await _AppDbContext.KOTs.AddAsync(kot);
        }

        public async Task<KOTEntity?> GetKOTByIdAsync(int kotId)
        {
            return await _AppDbContext.KOTs
                .Include(x => x.KOTItems)
                .ThenInclude(x => x.MenuItem)
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x => x.KOTId == kotId);
        }

        public async Task<List<KOTEntity>> GetRunningKOTsAsync()
        {
            return await _AppDbContext.KOTs
                .Include(x => x.KOTItems)
                .Include(x => x.RestaurantTable)
                .Where(x =>
                    x.Status == KOTStatus.New ||
                    x.Status == KOTStatus.Preparing)
                .OrderBy(x => x.GeneratedAt)
                .ToListAsync();
        }

        public async Task<List<KOTEntity>> GetKitchenQueueAsync()
        {
            return await _AppDbContext.KOTs
                .Include(x => x.KOTItems)
                    .ThenInclude(x => x.MenuItem)
                .Include(x => x.RestaurantTable)
                .Where(x =>
                    x.Status == KOTStatus.New ||
                    x.Status == KOTStatus.Preparing)
                .OrderBy(x => x.GeneratedAt)
                .ToListAsync();
        }

        public async Task UpdateKOTStatusAsync(
            int kotId,
            KOTStatus status)
        {
            var kot = await _AppDbContext.KOTs
                .FirstOrDefaultAsync(x => x.KOTId == kotId);

            if (kot == null)
            {
                return;
            }

            kot.Status = status;

            switch (status)
            {
                case KOTStatus.Preparing:
                    kot.PreparingAt = DateTime.UtcNow;
                    break;

                case KOTStatus.Ready:
                    kot.ReadyAt = DateTime.UtcNow;
                    break;

                case KOTStatus.Served:
                    kot.ServedAt = DateTime.UtcNow;
                    break;
            }

            _AppDbContext.KOTs.Update(kot);
        }

        public async Task<string> GenerateKOTNumberAsync()
        {
            var today = DateTime.UtcNow.Date;

            var todayCount = await _AppDbContext.KOTs
                .CountAsync(x =>
                    x.GeneratedAt.Date == today);

            var nextNumber = todayCount + 1;

            return $"KOT-{today:yyyyMMdd}-{nextNumber:D4}";
        }

    }
}
