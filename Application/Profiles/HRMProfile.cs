using AutoMapper;
using Domain.DTOs.Security.User;
using Domain.Entities.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class HRMProfile:Profile
    {
        public HRMProfile()
        {
            CreateMap<UserRegisterVM,User>();
        }
    }
}
