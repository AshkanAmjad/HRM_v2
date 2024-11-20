using AutoMapper;
using Domain.DTOs.General;
using Domain.DTOs.Security.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.HRMProfiles
{
    public class ReportProfile:Profile
    {
        public ReportProfile()
        {
            CreateMap<DisplayReportVM, AreaVM>()
                .ForMember(dest => dest.Section, opt => opt.MapFrom(src => src.Section))
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Province))
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.County))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District));

        }
    }
}
