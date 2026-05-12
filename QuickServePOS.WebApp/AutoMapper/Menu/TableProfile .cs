using AutoMapper;
using QuickServePOS.Models.DTO.Floor;
using QuickServePOS.Models.DTO.RestaurantTable;
using QuickServePOS.Models.Entities.Table;

namespace QuickServePOS.WebApp.AutoMapper.Menu
{
    public class TableProfile : Profile
    {
        public TableProfile()
        {
            // =========================
            // Floor
            // =========================

            CreateMap<FloorEntity, FloorListDto>()
                .ForMember(
                    dest => dest.TotalTables,
                    opt => opt.MapFrom(src => src.Tables.Count));

            CreateMap<FloorCreateDto, FloorEntity>();

            CreateMap<FloorUpdateDto, FloorEntity>();

            CreateMap<FloorEntity, FloorUpdateDto>();


            // =========================
            // RestaurantTable
            // =========================

            CreateMap<RestaurantTableEntity, TableListDto>()
                .ForMember(
                    dest => dest.FloorName,
                    opt => opt.MapFrom(src => src.Floor.Name));

            CreateMap<TableCreateDto, RestaurantTableEntity>();

            CreateMap<TableUpdateDto, RestaurantTableEntity>();

            CreateMap<RestaurantTableEntity, TableUpdateDto>();
        }
    }
}
