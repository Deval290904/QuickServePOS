using AutoMapper;
using QuickServePOS.Models.DTO.Floor;
using QuickServePOS.Models.DTO.RestaurantTable;
using QuickServePOS.Models.ViewModel.Table;

namespace QuickServePOS.WebApp.AutoMapper.MVCSideMapper.TableMVCMapper
{
    public class TableViewModelProfile : Profile
    {
        public TableViewModelProfile()
        {
            //Floor
            CreateMap<FloorListDto, FloorViewModel>().ReverseMap();

            CreateMap<FloorCreateDto, CreateFloorViewModel>().ReverseMap();

            CreateMap<FloorUpdateDto, UpdateFloorViewModel>().ReverseMap();


            // TABLE
            CreateMap<TableListDto, TableListViewModel>().ReverseMap();

            CreateMap<TableCreateDto, CreateTableViewModel>().ReverseMap();

            CreateMap<TableUpdateDto, UpdateTableViewModel>().ReverseMap();
        }
    }
}
