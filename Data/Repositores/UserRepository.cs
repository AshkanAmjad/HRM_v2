using AutoMapper;
using Data.Context;
using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.User;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositores
{
    public class UserRepository : IUserRepository
    {
        #region Constructor
        private readonly HRMContext _context;
        private readonly IMapper _mapper;

        public UserRepository(HRMContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion
        public async Task<User?> GetUserAsync(LoginVM loginVM)
        {
            var user = new User();

            if (loginVM.Area == "0")
            {
                user = await _context.Users.SingleOrDefaultAsync(
                   u => u.UserName == loginVM.UserName
                   &&
                   u.Password == loginVM.Password
                   &&
                   u.Department.Province != 0
                   &&
                   u.Department.County == 0
                   &&
                   u.Department.District == 0);
            }
            if (loginVM.Area == "1")
            {
                user = await _context.Users.SingleOrDefaultAsync(
                   u => u.UserName == loginVM.UserName
                   &&
                   u.Password == loginVM.Password
                   &&
                   u.Department.Province == 0
                   &&
                   u.Department.County != 0
                   &&
                   u.Department.District == 0);
            }
            if (loginVM.Area == "2")
            {
                user = await _context.Users.SingleOrDefaultAsync(
                    u => u.UserName == loginVM.UserName
                    &&
                    u.Password == loginVM.Password
                    &&
                    u.Department.Province == 0
                    &&
                    u.Department.County != 0
                    &&
                    u.Department.District != 0);
            }
            return user;
        }
        public bool Register(UserRegisterVM userRegister, out string message)
        {
            string checkMessage = "";
            if (Similarity(userRegister, out checkMessage) == false)
            {
                User user=_mapper.Map<User>(userRegister);
                user.IsActived= true;
                user.RegisterDate = DateTime.Now;
                user.LastActived= DateTime.Now;
                _context.Add(user);
                //Implement insert to the document table Insert
                _context.SaveChanges();
                message = "";
                return true;
            }
            message = checkMessage;
            return false;
        }

        public bool Similarity(UserRegisterVM userRegister, out string message)
        {
            bool check = false;
            var resultMessage = "";
            if (userRegister != null)
            {
                if (userRegister.Area == 0)
                {
                    check = _context.Users.Where(u => (u.UserName == userRegister.UserName) && (u.Department.Province == 1)).Any();
                }
                if (check)
                {
                    resultMessage = "کاربر" + userRegister.FirstName + " " + userRegister.LastName + "قبلا ثبت شده است.";
                }
            }
            message = resultMessage;
            return check;
        }
        public async Task<List<DisplayUsersVM>> GetUsersAsync()
        {
            var users= from item in _context.Users.AsNoTracking()
                   where (item.IsActived)
                   select new DisplayUsersVM
                   {
                       UserId = item.UserId,
                       UserName = item.UserName,
                       FirstName = item.FirstName,
                       LastName = item.LastName,
                       Gender = (item.Gender == "M") ? "مرد" : "زن",
                       MaritalStatus = (item.MaritalStatus == "S") ? "مجرد" : "متاهل",
                       Employment = (item.Employment == "T") ? "آزمایشی" : ((item.Employment == "C") ? "قراردادی" : "رسمی"),
                       Area = (item.Department.Province != 0 && item.Department.County == 0 && item.Department.District == 0) ? "استان" : (item.Department.Province != 0 && item.Department.County != 0 && item.Department.District == 0 ? "شهرستان" : "بخش"),
                       Department = (item.Department.Province != 0 && item.Department.County == 0 && item.Department.District == 0) ? (item.Department.Province) : (item.Department.Province != 0 && item.Department.County != 0 && item.Department.District == 0 ? item.Department.County : item.Department.District),
                       Insurance = (item.Insurance) ? "مشمول" : "در انتظار",
                       Education = (item.Education == "Dip") ? "دیپلم" : ((item.Education == "B") ? "کارشناسی" : ((item.Education == "M") ? "کارشناسی ارشد" : "دکترا")),
                       PhoneNumber = item.PhoneNumber,
                       Email = item.Email,
                       DateOfBirth = item.DateOfBirth,
                       City = item.City,
                       Address = item.Address,
                       IsActived = item.IsActived,
                       LastActived = item.LastActived,
                       RegisterDate = item.RegisterDate
                   };
            return await users.ToListAsync();
        }



    }
}
