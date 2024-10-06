using AutoMapper;
using Data.Context;
using Data.Extensions;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
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
            return _context.Users.IgnoreQueryFilters()
                                 .AsQueryable();
        }

        public User? GetUser(LoginVM model)
        {

            var user = new User();

            if (model.Area == "0")
            {
                user = _context.Users.SingleOrDefault(
                   u => u.UserName == model.UserName
                   &&
                   u.Department.Province != "0"
                   &&
                   u.Department.County == "0"
                   &&
                   u.Department.District == "0");
            }
            if (model.Area == "1")
            {
                user = _context.Users.SingleOrDefault(
                   u => u.UserName == model.UserName
                   &&
                   u.Department.Province != "0"
                   &&
                   u.Department.County != "0"
                   &&
                   u.Department.District == "0");
            }
            if (model.Area == "2")
            {
                user = _context.Users.SingleOrDefault(
                    u => u.UserName == model.UserName
                    &&
                    u.Department.Province != "0"
                    &&
                    u.Department.County != "0"
                    &&
                    u.Department.District != "0");
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

                UploadEditDepartmentToDb(user);

                #region Document
                if (user.Avatar != null)
                {
                    bool IsExist = IsExistAvatar(user.DepartmenyId);

                    if (IsExist)
                    {
                        DeleteAvatarOnDb(user.DepartmenyId);
                    }

                    UploadVM file = _mapper.Map<UploadVM>(user);

                    file.Name = "Avatar";
                    file.Description = "-";
                    file.DepartmentId = user.DepartmenyId;
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
            => _context.Documents.IgnoreQueryFilters()
                                 .Where(d => d.DepartmentId == departmentId)
                                 .Any();


        public Guid? GetDepartmentIdByUserId(Guid userId, string area)
        {
            var department = _context.Departments
                                     .IgnoreQueryFilters()
                                     .FirstOrDefault(d => d.UserId == userId && d.Area == area);

            return department?.DepartmentId;
        }

        public bool Similarity(UserRegisterVM user, out string message)
        {
            bool check = false;
            var resultMessage = "";
            if (user != null)
            {
                check = _context.Users.IgnoreQueryFilters()
                                      .Where(u => u.UserName == user.UserName && u.Department.Area == user.AreaDepartment)
                                      .Any();

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
                         where (item.Department.Area == area.Section && item.IsActived)
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
                             Area = (item.Department.Area == "0" ? "استان" : (item.Department.Area == "1" ? "شهرستان" : "بخش")),
                             County = item.Department.County,
                             District = item.Department.District,
                             Insurance = (item.Insurance) ? "مشمول" : "در انتظار",
                             Education = (item.Education == "Dip") ? "دیپلم" : ((item.Education == "B") ? "کارشناسی" : ((item.Education == "M") ? "کارشناسی ارشد" : "دکترا")),
                             PhoneNumber = item.PhoneNumber,
                             Email = item.Email,
                             DateOfBirth = item.DateOfBirth,
                             City = item.City,
                             Address = item.Address,
                             IsActived = (item.IsActived) ? "فعال" : "غیرفعال",
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

        public void UploadEditDepartmentToDb(UserEditVM model)
        {
            if (model != null)
            {
                var initial = _context.Departments.Find(model.DepartmenyId);

                if (initial != null)
                {
                    Department department = _mapper.Map<Department>(model);

                    initial.Province = department.Province;
                    initial.County = department.County;
                    initial.District = department.District;

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

        public UserEditVM? GetUserById(Guid userId)
        {
            UserEditVM user = new();
            if (userId != Guid.Empty)
            {
                user = (from item in _context.Users
                        where (item.UserId == userId)
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
                            DepartmenyId = item.Department.DepartmentId,
                            Insurance = item.Insurance,
                            Gender = item.Gender,
                            MaritalStatus = item.MaritalStatus,
                            EmploymentStatus = item.Employment,
                            CountyDepartment = item.Department.County,
                            DistrictDepartment = item.Department.District,
                            ProvinceDepartment = item.Department.Province,
                            AreaDepartment = (item.Department.Province != "0" && item.Department.County == "0" && item.Department.County == "0" ? "0" :
                                 (item.Department.Province != "0" && item.Department.County != "0" && item.Department.District == "0" ? "1" : "2"))
                        }).Single();
            }
            return user;
        }

        public bool Disable(UserEdit_DisableVM model, out string message)
        {
            string checkMessage = "عملیات غیر فعال سازی با شکست مواجه شد.";
            Guid _departmentId = Guid.Empty;

            if (model != null)
            {

                DisableUserDb(model);

                DisableDepartmentDb(model, out _departmentId);

                Guid departmentId = _departmentId;

                _documentRepository.DisableDocumentsDb(departmentId);

                message = "";

                return true;

            }
            message = checkMessage;
            return false;
        }

        public bool Active(UserDelete_ActiveVM model, out string message)
        {
            string checkMessage = "عملیات فعال سازی با شکست مواجه شد.";
            Guid _departmentId = Guid.Empty;

            if (model != null)
            {
                ActiveUserDb(model);

                ActiveDepartmentDb(model, out _departmentId);

                Guid departmentId = _departmentId;

                _documentRepository.ActiveDocumentsDb(departmentId);

                message = "";
                return true;
            }

            message = checkMessage;
            return false;
        }


        public void DisableUserDb(UserEdit_DisableVM model)
        {

            if (model != null)
            {
                var user = _context.Users
                                   .Find(model.UserId);

                if (user != null)
                {
                    user.IsActived = false;
                    user.RegisterDate = DateTime.Now;

                    _context.Users.Update(user);
                }
            }
        }

        public async void ActiveUserDb(UserDelete_ActiveVM model)
        {
            if (model != null)
            {
                var user = _context.Users.IgnoreQueryFilters()
                                         .Where(u=> u.UserId == model.UserId)
                                         .FirstOrDefault();

                if (user != null)
                {
                    user.IsActived = true;
                    user.RegisterDate = DateTime.Now;

                    _context.Users.Update(user);
                }
            }
        }

        public void DisableDepartmentDb(UserEdit_DisableVM model, out Guid departmentId)
        {
            Guid id = Guid.Empty;
            Department? department = null;

            if (model != null)
            {
                var _departmentId = GetDepartmentIdByUserId(model.UserId, model.Area);

                if (_departmentId != Guid.Empty)
                {
                    department = _context.Departments.Find(_departmentId);
                }

                if (department != null)
                {
                    department.IsActived = false;
                    department.RegisterDate = DateTime.Now;

                    id = department.DepartmentId;

                    _context.Departments.Update(department);
                }
            }

            departmentId = id;
        }

        public void ActiveDepartmentDb(UserDelete_ActiveVM model, out Guid departmentId)
        {
            Guid id = Guid.Empty;
            Department? department = null;

            if (model != null)
            {
                var _departmentId = GetDepartmentIdByUserId(model.UserId, model.Area);

                if( _departmentId != Guid.Empty)
                {
                    department = _context.Departments.IgnoreQueryFilters()
                                                     .Where(d => d.DepartmentId == _departmentId)
                                                     .FirstOrDefault();
                }

                if (department != null)
                {
                    department.IsActived = true;
                    department.RegisterDate = DateTime.Now;

                    id = department.DepartmentId;

                    _context.Departments.Update(department);
                }
            }

            departmentId = id;
        }

        public List<DisplayUsersVM> GetArchivedUsers(AreaVM area)
        {
            var context = GetUsersQuery();
            var users = (from item in context
                         where (item.Department.Area == area.Section && !item.IsActived)
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
                             Area = (item.Department.Area == "0" ? "استان" : (item.Department.Area == "1" ? "شهرستان" : "بخش")),
                             County = item.Department.County,
                             District = item.Department.District,
                             Insurance = (item.Insurance) ? "مشمول" : "در انتظار",
                             Education = (item.Education == "Dip") ? "دیپلم" : ((item.Education == "B") ? "کارشناسی" : ((item.Education == "M") ? "کارشناسی ارشد" : "دکترا")),
                             PhoneNumber = item.PhoneNumber,
                             Email = item.Email,
                             DateOfBirth = item.DateOfBirth,
                             City = item.City,
                             Address = item.Address,
                             IsActived = (item.IsActived) ? "فعال" : "غیرفعال",
                             LastActived = $"{item.LastActived.ToShamsi()}",
                             RegisterDate = $"{item.RegisterDate.ToShamsi()}"
                         })
                         .ToList();

            return users;
        }

        public bool Delete(UserDelete_ActiveVM user, out string message)
        {
            string checkMessage = "انجام عملیات حذف با شکست مواحه شد.";
            if (user == null)
            {
                message = checkMessage;
                return false;
            }

            var context = GetUsersQuery();

            if (context == null)
            {
                message = checkMessage;
                return false;
            }

            var data = context.Where(u => u.UserId == user.UserId)
                              .FirstOrDefault();

            if (data == null)
            {
                message = checkMessage;
                return false;
            }

            _context.Remove(data);

            message = "";
            return true;
        }
    }
}
