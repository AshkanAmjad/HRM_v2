using AutoMapper;
using Data.Context;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.User;
using Domain.Entities.Portal.Models;
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
            if (!Similarity(userRegister, out checkMessage))
            {
                #region User
                User user =_mapper.Map<User>(userRegister);
                #endregion

                #region Department
                Department department =_mapper.Map<Department>(userRegister);
                var departmentId=Guid.NewGuid();
                department.DepartmentId=departmentId;
                #endregion

                _context.Add(user);
                _context.Add(department);

                #region Document
                UploadVM file = _mapper.Map<UploadVM>(userRegister);
                file.Name = "Avatar";
                file.Description = "-";
                file.DepartmentId = departmentId;
                UploadDocumentToDb(file);
                #endregion

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
                    resultMessage = $"کاربر با کدملی {userRegister.UserName} قبلا ثبت شده است.";
                }
            }
            message = resultMessage;
            return check;
        }
        public async Task<List<DisplayUsersVM>> GetUsersAsync(AreaVM area)
        {
            var users= from item in _context.Users.AsNoTracking()
                   where (item.IsActived && item.Department.Province == area.Province && item.Department.County == area.County && item.Department.District == area.District)
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
                       County = item.Department.County,
                       District = item.Department.District,
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

        public void UploadDocumentToDb(UploadVM file)
        {
            if (file.document != null && file.document.Length>0)
            {
                var userName = file.UserName;
                Document document = _mapper.Map<Document>(file);
                document.IsActived = true;
                document.UploadDate=DateTime.Now;
                document.DocumentId=Guid.NewGuid();
                var docName = Path.GetFileName(file.document.FileName);
                var fileExtension = Path.GetExtension(docName);
                document.FileName = string.Concat($"{file.Name}-{userName}",fileExtension);
                document.FileFormat = fileExtension;
                using (var target = new MemoryStream())
                {
                    file.document.CopyTo(target);
                    document.DataBytes = target.ToArray();
                }
                _context.Add(document);
            }

        }

    }
}
