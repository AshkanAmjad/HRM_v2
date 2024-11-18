using AutoMapper;
using Domain.DTOs.Security.UserRole;
using Domain.Entities.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.HRMProfiles
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<UserRoleRegisterVM, UserRole>()
                .ForMember(dest => dest.IsActived, opt => opt.MapFrom(src => src.IsActived))
                .ForMember(dest => dest.UserRoleId, opt => opt.MapFrom(src => src.UserRoleId))
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => src.RegisterDate))
                .ForMember(dest => dest.IsActived, opt => opt.MapFrom(src => src.IsActived));

  
        }
    }
}
