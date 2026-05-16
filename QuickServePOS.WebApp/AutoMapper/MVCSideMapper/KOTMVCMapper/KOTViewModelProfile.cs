using AutoMapper;
using QuickServePOS.Models.DTO.KOT;
using QuickServePOS.Models.ViewModel.KOT;

namespace QuickServePOS.WebApp.AutoMapper.MVCSideMapper.KOTMVCMapper
{
    public class KOTViewModelProfile : Profile
    {
        public KOTViewModelProfile()
        {
            CreateMap<KitchenQueueDto,KitchenQueueViewModel>();


            CreateMap<KOTDetailsDto,KOTDetailsViewModel>();


            CreateMap<KOTItemDetailsDto,KOTItemViewModel>();

            CreateMap<UpdateKOTItemStatusViewModel,UpdateKOTItemStatusDto>();
        }
    }
}
