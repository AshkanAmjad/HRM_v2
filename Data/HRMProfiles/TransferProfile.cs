using AutoMapper;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Portal.Transfer;
using Domain.Entities.Portal.Models;
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
               .ForMember(dest => dest.Document, opt => opt.MapFrom(src => src.Document));

            CreateMap<TransferRegisterVM, Transfer>()
                .ForMember(dest => dest.TransferId, opt => opt.MapFrom(src => src.TransferId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.Document.ContentType))
                .ForMember(dest => dest.UploadDate, opt => opt.MapFrom(src => src.UploadDate))
                .ForMember(dest => dest.IsActived, opt => opt.MapFrom(src => src.IsActived));

        }
    }
}
