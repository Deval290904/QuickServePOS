using AutoMapper;
using QuickServePOS.Models.DTO.Menu;
using QuickServePOS.Models.ViewModel.Menu;

namespace QuickServePOS.WebApp.AutoMapper.MVCSideMapper.MenuMVCMapper
{
    public class CategoryViewModelProfile : Profile
    {
        public CategoryViewModelProfile()
        {
            CreateMap<CategoryDto, CategoryViewModel>().ReverseMap();

            CreateMap<CreateCategoryDto, CreateCategoryViewModel>().ReverseMap();

            CreateMap<CategoryDto, UpdateCategoryViewModel>().ReverseMap();

            CreateMap<UpdateCategoryDto, UpdateCategoryViewModel>().ReverseMap();
        }
    }
}
