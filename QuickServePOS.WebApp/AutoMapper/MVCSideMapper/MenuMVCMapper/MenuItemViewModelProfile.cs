using AutoMapper;
using QuickServePOS.Models.DTO.Menu;
using QuickServePOS.Models.DTO.Profile;
using QuickServePOS.Models.ViewModel.Menu;

namespace QuickServePOS.WebApp.AutoMapper.MVCSideMapper.MenuMVCMapper
{
    public class MenuItemViewModelProfile : Profile
    {
        public MenuItemViewModelProfile()
        {
            // LIST
            CreateMap<MenuItemDto, MenuItemViewModel>().ReverseMap();

            // CREATE
            CreateMap<CreateMenuItemDto,  CreateMenuItemViewModel>().ReverseMap();

            // UPDATE DTO ↔ VIEWMODEL
            CreateMap<UpdateMenuItemDto,UpdateMenuItemViewModel>().ReverseMap();

            // EDIT GET
            CreateMap<MenuItemDto,UpdateMenuItemViewModel>().ReverseMap();
        }
    }
}
