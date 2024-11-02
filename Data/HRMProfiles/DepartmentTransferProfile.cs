using AutoMapper;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.HRMProfiles
{
    public class DepartmentTransferProfile : Profile
    {
        public DepartmentTransferProfile()
        {
            CreateMap<TransferRegisterVM, AreaVM>()
                .ForMember(dest => dest.Section, opt => opt.MapFrom(src => src.AreaReceiver))
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.ProvinceReceiver))
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.CountyReceiver))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.DistrictReceiver));
        }
    }
}
