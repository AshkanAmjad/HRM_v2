using AutoMapper;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.Profile;
using Domain.DTOs.Security.User;
using Domain.DTOs.Security.UserRole;
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
    public class DocumentProfle : Profile
    {
        public DocumentProfle()
        {
            CreateMap<UserRegisterVM, UploadVM>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.AreaDepartment))
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.CountyDepartment))
                .ForMember(dest => dest.District, opt => opt.MapFrom(opt => opt.DistrictDepartment))
                .ForMember(dest => dest.Document, opt => opt.MapFrom(opt => opt.Avatar));

            CreateMap<UserEditVM, UploadVM>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmenyId))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.AreaDepartment))
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.CountyDepartment))
                .ForMember(dest => dest.District, opt => opt.MapFrom(opt => opt.DistrictDepartment))
                .ForMember(dest => dest.Document, opt => opt.MapFrom(opt => opt.Avatar));

            CreateMap<ProfileEditVM, UploadVM>()
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.UserName))
               .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmenyId))
               .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.AreaDepartment))
               .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.CountyDepartment))
               .ForMember(dest => dest.District, opt => opt.MapFrom(opt => opt.DistrictDepartment))
               .ForMember(dest => dest.Document, opt => opt.MapFrom(opt => opt.Avatar));

            CreateMap<UploadVM, DirectionVM>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.County))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District));

            CreateMap<UploadVM, Document>()
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.Document.ContentType))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<UserEditVM, DirectionVM>()
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.CountyDepartment))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.AreaDepartment))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.DistrictDepartment));

            CreateMap<ProfileEditVM,DirectionVM>()
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.CountyDepartment))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.AreaDepartment))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.DistrictDepartment));

            CreateMap<DisplayProfileVM,DirectionVM>()
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.CountyDepartment))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.AreaDepartment))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.DistrictDepartment));

            CreateMap<DisplayDetailsVM, DirectionVM>()
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.CountyDepartment))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.AreaDepartment))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.DistrictDepartment));


            CreateMap<UserDelete_ActiveVM, DirectionVM>()
                 .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.County))
                 .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
                 .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District));

            CreateMap<Document, DownloadAvatarVM>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.DataBytes, opt => opt.MapFrom(src => src.DataBytes));

            CreateMap<Document, DirectionVM>()
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
                 .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.Department.County))
                 .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.Department.District))
                 .ForMember(dest => dest.Area, opt => opt.MapFrom(src => GetArea(src.Department)));

            CreateMap<UserEdit_DisableVM, DirectionVM>()
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.County))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => GetArea(src.Province, src.County, src.District)));
        }

        private int GetArea(Department department)
        {
            if (department.Province != "0" && department.County == "0" && department.District == "0")
                return 0;
            else if (department.Province != "0" && department.County != "0" && department.District == "0")
                return 1;
            else
                return 2;
        }

        private int GetArea(string? province, string? county, string? district)
        {
            if (province != "0" && county == "0" && district == "0")
                return 0;
            else if (province != "0" && county != "0" && district == "0")
                return 1;
            else
                return 2;
        }
    }
}
