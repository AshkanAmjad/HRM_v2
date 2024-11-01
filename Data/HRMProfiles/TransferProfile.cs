using AutoMapper;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Portal.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.HRMProfiles
{
    public class TransferProfile:Profile
    {
        public TransferProfile()
        {
            CreateMap<TransferRegisterVM, UploadVM>()
               .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.AreaUploader))
               .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.CountyReceiver))
               .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.DistrictReceiver))
               .ForMember(dest => dest.document, opt => opt.MapFrom(src => src.Document));
        }
    }
}
