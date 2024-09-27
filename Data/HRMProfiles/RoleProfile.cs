using AutoMapper;
using Domain.DTOs.Security.Role;
using Domain.Entities.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.HRMProfiles
{
    public class RoleProfile:Profile
    {
        public RoleProfile()
        {
            CreateMap<AssistantRegisterVM, Role>()
                .ForMember(dest => dest.IsActived, opt => opt.MapFrom(src => src.IsActived))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => src.RegisterDate));

            CreateMap<AssistantEditVM, Role>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => src.RegisterDate));
        }
    }
}
