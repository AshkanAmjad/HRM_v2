using Application.Services.Interfaces;
using Data.Context;
using Domain.DTOs.Enums;
using Domain.DTOs.Security.Login;
using Domain.Entities.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Mvc;

namespace Application.Services.Implrmentations
{
    public class UserService:IUserService
    {
        #region Constructor
        private readonly HRMContext _context;

        public UserService(HRMContext context)
        {
            _context = context;
        }
        #endregion
        public List<SelectListItem> GetAreas()
        {
            List<SelectListItem> areas = new()
            {
                new SelectListItem
                {
                    Text = "استان",
                    Value = 0.ToString()
                },
                new SelectListItem
                {
                    Text = "شهرستان",
                    Value = 1.ToString()
                },
                new SelectListItem
                {
                    Text = "بخش",
                    Value = 3.ToString()
                }
            };
            return areas;
        }

        public async Task<User?> GetUser(LoginVM loginDTO)
        {
            var userName= loginDTO.UserName;
            var password=loginDTO.Password;
            var user=new User();
            if (loginDTO.Area.Equals('0'))
            {
                 user =await _context.Users.SingleOrDefaultAsync(
                    u => u.UserName == userName
                    &&
                    u.Password == password
                    &&
                    u.Department.Province!=0
                    &&
                    u.Department.County==0
                    &&
                    u.Department.District==0);
            }
            if(loginDTO.Area.Equals('1'))
            {
                 user =await _context.Users.SingleOrDefaultAsync(
                    u => u.UserName == userName
                    &&
                    u.Password == password
                    &&
                    u.Department.Province == 0
                    &&
                    u.Department.County != 0
                    &&
                    u.Department.District == 0);
            }
            if (loginDTO.Area.Equals('2'))
            {
                 user =await _context.Users.SingleOrDefaultAsync(
                    u => u.UserName == userName
                    &&
                    u.Password == password
                    &&
                    u.Department.Province == 0
                    &&
                    u.Department.County != 0
                    &&
                    u.Department.District != 0);
            }
            return user;
        }
    


    }
}
