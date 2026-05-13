using AutoMapper;
using QuickServePOS.Models.DTO.Order;
using QuickServePOS.Models.Entities.Order;

namespace QuickServePOS.WebApp.AutoMapper.APISideMapper.OrderAPIMapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // =========================
            // Order
            // =========================

            CreateMap<OrderEntity, OrderListDto>()
                .ForMember(dest => dest.TableName,
                    opt => opt.MapFrom(src =>
                        src.Table != null
                            ? src.Table.TableNumber
                            : null));

            CreateMap<OrderEntity, OrderDetailsDto>()
                .ForMember(dest => dest.TableName,
                    opt => opt.MapFrom(src =>
                        src.Table != null
                            ? src.Table.TableNumber
                            : null))

                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src =>
                        src.OrderItems));

            CreateMap<OrderCreateDto, OrderEntity>();

            CreateMap<OrderUpdateDto, OrderEntity>();

            // =========================
            // Order Item
            // =========================

            CreateMap<OrderItemEntity, OrderItemDetailsDto>()
                .ForMember(dest => dest.MenuItemName,
                    opt => opt.MapFrom(src =>
                        src.MenuItem.Name));

            CreateMap<OrderItemCreateDto, OrderItemEntity>();

            CreateMap<OrderItemUpdateDto, OrderItemEntity>();
        }
    }
    
}
