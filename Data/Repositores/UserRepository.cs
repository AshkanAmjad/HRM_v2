using AutoMapper;
using Data.Context;
using Data.Extensions;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.User;
using Domain.Entities.Portal.Models;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositores
{
    public class UserRepository : IUserRepository
    {
        #region Constructor
        private readonly HRMContext _context;
        private readonly IMapper _mapper;
        private readonly IDocumentRepository _documentRepository;
        public UserRepository(HRMContext context,
            IMapper mapper,
            IDocumentRepository documentRepository)
        {
            _context = context;
            _mapper = mapper;
            _documentRepository = documentRepository;
        }
        #endregion

        public IQueryable<User> GetUsersQuery()
        {
            return _context.Users.AsQueryable();
        }

        public async Task<User?> GetUserAsync(LoginVM model)
        {

            var user = new User();

            if (model.Area == "0")
            {
                user = await _context.Users.SingleOrDefaultAsync(
                   u => u.UserName == model.UserName
                   &&
                   u.Password == model.Password
                   &&
                   u.Department.Province != 0
                   &&
                   u.Department.County == 0
                   &&
                   u.Department.District == 0);
            }
            if (model.Area == "1")
            {
                user = await _context.Users.SingleOrDefaultAsync(
                   u => u.UserName == model.UserName
                   &&
                   u.Password == model.Password
                   &&
                   u.Department.Province == 0
                   &&
                   u.Department.County != 0
                   &&
                   u.Department.District == 0);
            }
            if (model.Area == "2")
            {
                user = await _context.Users.SingleOrDefaultAsync(
                    u => u.UserName == model.UserName
                    &&
                    u.Password == model.Password
                    &&
                    u.Department.Province == 0
                    &&
                    u.Department.County != 0
                    &&
                    u.Department.District != 0);
            }
            return user;
        }
        public bool Register(UserRegisterVM user, out string message)
        {
            string checkMessage = "";
            if (!Similarity(user, out checkMessage))
            {

                UploadRegisterUserToDb(user);

                var departmentId = Guid.NewGuid();

                UploadDepartmentToDb(user, departmentId);

                #region Document
                if (user.Avatar != null)
                {
                    UploadVM file = _mapper.Map<UploadVM>(user);
                    file.Name = "Avatar";
                    file.Description = "-";
                    file.DepartmentId = departmentId;
                    _documentRepository.UploadDocumentToDb(file);
                }
                #endregion

                message = "";
                return true;
            }
            message = checkMessage;
            return false;
        }

        public bool Edit(UserEditVM user, out string message)
        {
            string checkMessage = "اطلاعات ناقص ارسال شده است.";

            if (user != null)
            {
                UploadEditUserToDb(user);

                #region Document
                if (user.Avatar != null)
                {
                    var departmentId = GetDepartmentId(user.UserId);

                    bool IsExist = IsExistAvatar(departmentId);

                    if (IsExist)
                    {
                        DeleteAvatarOnDb(departmentId);
                    }

                    UploadVM file = _mapper.Map<UploadVM>(user);

                    file.Name = "Avatar";
                    file.Description = "-";
                    file.DepartmentId = departmentId;
                    _documentRepository.UploadDocumentToDb(file);
                }
                #endregion

                message = "";
                return true;
            }

            message = checkMessage;
            return false;
        }

        public void DeleteAvatarOnDb(Guid departmentId)
        {
            if (departmentId != Guid.Empty)
            {
                var avatar = _context.Documents.Where(d => d.DepartmentId == departmentId)
                                               .Single();

                _context.Documents.Remove(avatar);

            }
        }


        public bool IsExistAvatar(Guid departmentId)
            => _context.Documents.Where(d => d.DepartmentId == departmentId)
                                 .Any();


        public Guid GetDepartmentId(Guid userId)
            => _context.Departments.Where(d => d.UserId == userId)
                                   .Select(d => d.DepartmentId)
                                   .Single();

        public bool Similarity(UserRegisterVM user, out string message)
        {
            bool check = false;
            var resultMessage = "";
            if (user != null)
            {
                if (user.Area == 0)
                {
                    check = _context.Users.Where(u => (u.UserName == user.UserName) && (u.Department.Province == 1)).Any();
                }
                if (check)
                {
                    resultMessage = $"کاربر با کدملی {user.UserName} قبلا ثبت شده است.";
                }
            }
            message = resultMessage;
            return check;
        }
        public List<DisplayUsersVM> GetUsers(AreaVM area)
        {
            var context = GetUsersQuery();
            var users = (from item in context
                         where (item.IsActived && item.Department.Province == area.Province && item.Department.County == area.County && item.Department.District == area.District)
                         orderby item.RegisterDate descending
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
                             IsActived = (item.IsActived == true) ? "فعال" : "غیرفعال",
                             LastActived = $"{item.LastActived.ToShamsi()}",
                             RegisterDate = $"{item.RegisterDate.ToShamsi()}"
                         })
                         .ToList();

            return users;
        }


        public void UploadRegisterUserToDb(UserRegisterVM userRegister)
        {
            if (userRegister != null)
            {
                User user = _mapper.Map<User>(userRegister);
                _context.Add(user);
            }

        }

        public void UploadEditUserToDb(UserEditVM userEdit)
        {
            if (userEdit != null)
            {
                User initial = _context.Users
                                       .Find(userEdit.UserId);
                if (initial != null)
                {
                    User user = _mapper.Map<User>(userEdit);

                    initial.FirstName = user.FirstName;
                    initial.LastName = user.LastName;
                    initial.Gender = user.Gender;
                    initial.Education = user.Education;
                    initial.Employment = user.Employment;
                    initial.MaritalStatus = user.MaritalStatus;
                    initial.Insurance = user.Insurance;
                    initial.PhoneNumber = user.PhoneNumber;
                    initial.Email = user.Email;
                    initial.DateOfBirth = user.DateOfBirth;
                    initial.City = user.City;
                    initial.Address = user.Address;
                    initial.IsActived = user.IsActived;
                    initial.LastActived = user.LastActived;
                    initial.RegisterDate = user.RegisterDate;

                    if (userEdit.Password != null)
                    {
                        user.Password = userEdit.Password;
                        initial.Password = user.Password;
                    }

                    _context.Update(initial);

                }

            }
        }

        public void UploadDepartmentToDb(UserRegisterVM userRegister, Guid departmentId)
        {
            if (userRegister != null)
            {
                Department department = _mapper.Map<Department>(userRegister);
                department.DepartmentId = departmentId;
                _context.Add(department);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public UserEditVM? GetUserById(Guid userId, AreaVM area)
        {
            UserEditVM user = new();
            if (userId != Guid.Empty)
            {
                user = (from item in _context.Users
                        where (item.UserId == userId
                        && item.Department.Province == area.Province
                        && item.Department.County == area.County
                        && item.Department.District == area.District
                        && item.IsActived == true)
                        select new UserEditVM
                        {
                            Address = item.Address,
                            City = item.City,
                            DateOfBirth = item.DateOfBirth,
                            Education = item.Education,
                            Email = item.Email,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            PhoneNumber = item.PhoneNumber,
                            UserId = item.UserId,
                            UserName = item.UserName,
                            Insurance = item.Insurance,
                            Gender = item.Gender,
                            MaritalStatus = item.MaritalStatus,
                            EmploymentStatus = item.Employment,
                            County = item.Department.County,
                            District = item.Department.District,
                            Province = item.Department.Province,
                            Area = (item.Department.Province != 0 && item.Department.County == 0 && item.Department.County == 0 ? 0 :
                                 (item.Department.Province != 0 && item.Department.County != 0 && item.Department.District == 0 ? 1 : 2)),
                            IsActived = item.IsActived
                        }).Single();
            }
            return user;
        }

        public bool Delete(UserDeleteVM model, out string message)
        {
            throw new NotImplementedException();
        }
    }
}
