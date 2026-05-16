using AutoMapper;
using QuickServePOS.Models.DTO.KOT;
using QuickServePOS.Models.Entities.Enums;
using QuickServePOS.Models.Entities.KOT;
using QuickServePOS.Repositories.IUnitofWork;
using QuickServePOS.Services.IService.KOT;

namespace QuickServePOS.Services.Service.KOT
{
    public class KOTService : IKOTService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public KOTService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task GenerateKOTAsync(int orderId)
        {
            var order = await _unitOfWork.Orders
                .GetOrderWithItemsAsync(orderId);

            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            var kotItems = new List<KOTItemEntity>();

            foreach (var orderItem in order.OrderItems)
            {
                var deltaQuantity =
                    orderItem.Quantity -
                    orderItem.ConfirmedQuantity;

                if (deltaQuantity <= 0)
                {
                    continue;
                }

                kotItems.Add(new KOTItemEntity
                {
                    OrderItemId = orderItem.Id,

                    MenuItemId = orderItem.MenuItemId,

                    Quantity = deltaQuantity,

                    PreparedQuantity = 0,

                    Status = KitchenItemStatus.Pending,

                    SpecialInstruction =
                        orderItem.SpecialInstruction
                });

                orderItem.ConfirmedQuantity =
                    orderItem.Quantity;

                orderItem.IsKOTGenerated = true;
            }

            if (!kotItems.Any())
            {
                return;
            }
            if (!order.TableId.HasValue)
            {
                throw new Exception("Table not assigned.");
            }


            var kotNumber =await _unitOfWork.KOTs .GenerateKOTNumberAsync();

            var kot = new KOTEntity
            {
                KOTNumber = kotNumber,

                OrderId = order.Id,

                RestaurantTableId = order.TableId.Value,

                Status = KOTStatus.New,

                GeneratedAt = DateTime.UtcNow,

                KOTItems = kotItems
            };

            await _unitOfWork.KOTs.AddKOTAsync(kot);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<KitchenQueueDto>> GetKitchenQueueAsync()
        {
            var kots = await _unitOfWork.KOTs
                .GetKitchenQueueAsync();

            return _mapper.Map<List<KitchenQueueDto>>(kots);
        }

        public async Task<KOTDetailsDto?>GetKOTByIdAsync(int kotId)
        {
            var kot = await _unitOfWork.KOTs
                .GetKOTByIdAsync(kotId);

            if (kot == null)
            {
                return null;
            }

            return _mapper.Map<KOTDetailsDto>(kot);
        }

        public async Task UpdateKOTStatusAsync(int kotId,KOTStatus status)
        {
            await _unitOfWork.KOTs
                .UpdateKOTStatusAsync(
                    kotId,
                    status);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
