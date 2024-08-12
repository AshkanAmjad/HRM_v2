using AutoMapper;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.User;
using Domain.Entities.Portal.Models;
using Domain.Entities.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Data.HRMProfiles
{
    public class DocumentProfle:Profile
    {
        public DocumentProfle()
        {
            CreateMap<UserRegisterVM, UploadVM>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.County))
                .ForMember(dest => dest.District, opt => opt.MapFrom(opt => opt.District))
                .ForMember(dest => dest.document, opt => opt.MapFrom(opt => opt.Avatar));

            CreateMap<UploadVM, DirectionVM>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.County))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District));

            CreateMap<UploadVM, Document>()
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.document.ContentType))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<UserEditVM, DirectionVM>()
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.County))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District));
        }
    }
}
