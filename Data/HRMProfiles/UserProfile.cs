using AutoMapper;
using Domain.DTOs.Security.User;
using Domain.Entities.Security.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.HRMProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegisterVM, User>()
                   .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                   .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                   .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                   .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                   .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                   .ForMember(dest => dest.MaritalStatus, opt => opt.MapFrom(src => src.MaritalStatus))
                   .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                   .ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.Education))
                   .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower()))
                   .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(opt => opt.DateOfBirth))
                   .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                   .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                   .ForMember(dest => dest.Employment, opt => opt.MapFrom(src => src.EmploymentStatus))
                   .ForMember(dest => dest.Insurance, opt => opt.MapFrom(src => src.Insurance))
                   .ForMember(dest => dest.IsActived, opt => opt.MapFrom(src => src.IsActived))
                   .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => src.RegisterDate))
                   .ForMember(dest => dest.LastActived, opt => opt.MapFrom(src => src.LastActived));

            CreateMap<UserRegisterVM, Department>()
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.AreaDepartment))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.ProvinceDepartment))
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.CountyDepartment))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.DistrictDepartment))
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => src.RegisterDate))
                .ForMember(dest => dest.IsActived, opt => opt.MapFrom(src => src.IsActived));

            CreateMap<UserEditVM, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                   .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                   .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                   .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                   .ForMember(dest => dest.MaritalStatus, opt => opt.MapFrom(src => src.MaritalStatus))
                   .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                   .ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.Education))
                   .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower()))
                   .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(opt => opt.DateOfBirth))
                   .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                   .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                   .ForMember(dest => dest.Employment, opt => opt.MapFrom(src => src.EmploymentStatus))
                   .ForMember(dest => dest.Insurance, opt => opt.MapFrom(src => src.Insurance))
                   .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => src.RegisterDate))
                   .ForMember(dest => dest.LastActived, opt => opt.MapFrom(src => src.LastActived));

            CreateMap<UserEditVM, Department>()
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.ProvinceDepartment))
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.CountyDepartment))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.DistrictDepartment));

        }
    }
}
