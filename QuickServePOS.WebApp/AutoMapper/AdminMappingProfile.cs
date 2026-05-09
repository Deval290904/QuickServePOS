using AutoMapper;
using QuickServePOS.Models.DTO.Admin;
using QuickServePOS.Models.ViewModel.Authentication;
using QuickServePOS.Models.ViewModel.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.AutoMapper
{
    public class AdminMappingProfile : Profile
    {
        public AdminMappingProfile()
        {
            CreateMap<CreateStaffViewModel, CreateStaffAccountDto>().ReverseMap();

            CreateMap<UpdateStaffViewModel, UpdateStaffDto>().ReverseMap();
        }
    }
}
