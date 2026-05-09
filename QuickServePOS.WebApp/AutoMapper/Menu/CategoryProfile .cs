using AutoMapper;
using QuickServePOS.Models.DTO.Menu;
using QuickServePOS.Models.Entities.Menu;

namespace QuickServePOS.WebApp.AutoMapper.Menu
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryEntity,CategoryDto>().ReverseMap();
            CreateMap<CategoryEntity,CreateCategoryDto>().ReverseMap();
            CreateMap<CategoryEntity,UpdateCategoryDto>().ReverseMap();
        }
    }
}
