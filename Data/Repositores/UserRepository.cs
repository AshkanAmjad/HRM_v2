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

        public UserRepository(HRMContext context)
        {
            _context = context;
        }
        #endregion
        public async Task<User?> GetUser(LoginVM loginVM)
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
                //Implement Register
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
    }
}
