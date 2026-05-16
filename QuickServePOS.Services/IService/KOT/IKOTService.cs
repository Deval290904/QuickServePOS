using QuickServePOS.Models.DTO.KOT;
using QuickServePOS.Models.Entities.Enums;
using QuickServePOS.Models.Entities.KOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.IService.KOT
{
    public interface IKOTService
    {
        Task GenerateKOTAsync(int orderId);

        Task<List<KitchenQueueDto>> GetKitchenQueueAsync();

        Task<KOTDetailsDto?> GetKOTByIdAsync(int kotId);

        Task UpdateKOTStatusAsync( int kotId,KOTStatus status);

        Task UpdateKOTItemStatusAsync(int kotItemId,KitchenItemStatus status);
    }
}