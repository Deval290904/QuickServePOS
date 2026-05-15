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

            CreateMap<OrderCreateDto, OrderEntity>();

            CreateMap<OrderUpdateDto, OrderEntity>();

            CreateMap<OrderEntity, OrderListDto>()
                .ForMember(
                    dest => dest.TableNumber,
                    opt => opt.MapFrom(src =>
                        src.Table != null
                            ? src.Table.TableNumber
                            : null));

            CreateMap<OrderEntity, OrderDetailsDto>()
                .ForMember(
                    dest => dest.TableNumber,
                    opt => opt.MapFrom(src =>
                        src.Table != null
                            ? src.Table.TableNumber
                            : null))

                .ForMember(
                    dest => dest.Items,
                    opt => opt.MapFrom(src => src.OrderItems));

            // =========================
            // OrderItem
            // =========================

            CreateMap<OrderItemCreateDto, OrderItemEntity>();

            CreateMap<UpdateCartItemDto, OrderItemEntity>();

            CreateMap<OrderItemEntity, OrderItemListDto>()
                .ForMember(
                    dest => dest.MenuItemName,
                    opt => opt.MapFrom(src =>
                        src.MenuItem.Name));
        }
    }
    
}
