using AutoMapper;
using QuickServePOS.Models.DTO.KOT;
using QuickServePOS.Models.Entities.KOT;

namespace QuickServePOS.WebApp.AutoMapper.APISideMapper.KOTAPIMapper
{
    public class KOTProfile : Profile
    {
        public KOTProfile()
        {
            CreateMap<KOTEntity, KitchenQueueDto>()
                .ForMember(
                    dest => dest.TableName,
                    opt => opt.MapFrom(
                        src => src.RestaurantTable.TableNumber))

                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(
                        src => src.Status.ToString()))

                .ForMember(
                    dest => dest.TotalItems,
                    opt => opt.MapFrom(
                        src => src.KOTItems.Count));



            CreateMap<KOTEntity, KOTDetailsDto>()
                .ForMember(
                    dest => dest.TableName,
                    opt => opt.MapFrom(
                        src => src.RestaurantTable.TableNumber))

                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(
                        src => src.Status.ToString()))

                .ForMember(
                    dest => dest.Items,
                    opt => opt.MapFrom(
                        src => src.KOTItems));



            CreateMap<KOTItemEntity, KOTItemDetailsDto>()
                .ForMember(
                    dest => dest.MenuItemName,
                    opt => opt.MapFrom(
                        src => src.MenuItem.Name))

                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(
                        src => src.Status.ToString()));
        }
     
    }
}
