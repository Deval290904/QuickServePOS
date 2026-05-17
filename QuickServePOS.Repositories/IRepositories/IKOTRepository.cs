using QuickServePOS.Models.Entities.Enums;
using QuickServePOS.Models.Entities.KOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Repositories.IRepositories
{
    public interface IKOTRepository
    {
        Task AddKOTAsync(KOTEntity kot);

        Task<KOTEntity?> GetKOTByIdAsync(int kotId);

        Task<List<KOTEntity>> GetRunningKOTsAsync();

        Task<List<KOTEntity>> GetKitchenQueueAsync();

        Task UpdateKOTStatusAsync(int kotId, KOTStatus status);

        Task<string> GenerateKOTNumberAsync();

        Task UpdateKOTItemStatusAsync(int kotItemId,KitchenItemStatus status);

        Task<List<KOTEntity>> GetReadyKOTsAsync();
    }
}
