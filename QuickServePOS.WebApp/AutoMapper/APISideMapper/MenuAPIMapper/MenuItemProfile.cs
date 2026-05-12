using AutoMapper;
using QuickServePOS.Models.DTO.Menu;
using QuickServePOS.Models.Entities.Menu;

namespace QuickServePOS.WebApp.AutoMapper.APISideMapper.MenuAPIMapper
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            // Entity -> DTO
            CreateMap<MenuItemEntity, MenuItemDto>()
                .ForMember(
                    dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name));

            // Create DTO -> Entity
            CreateMap<CreateMenuItemDto, MenuItemEntity>()
                .ForMember(
                    dest => dest.ImageUrl,
                    opt => opt.Ignore());

            // Update DTO -> Entity
            CreateMap<UpdateMenuItemDto, MenuItemEntity>()
                .ForMember(
                    dest => dest.ImageUrl,
                    opt => opt.Ignore());

            // Entity -> Update DTO
            CreateMap<MenuItemEntity, UpdateMenuItemDto>()
                .ForMember(
                    dest => dest.ExistingImageUrl,
                    opt => opt.MapFrom(src => src.ImageUrl));
        }
    }

}
