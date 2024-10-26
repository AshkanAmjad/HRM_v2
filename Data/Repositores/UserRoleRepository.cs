using AutoMapper;
using Data.Context;
using Data.Extensions;
using Domain.DTOs.Security.User;
using Domain.DTOs.Security.UserRole;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositores
{
    public class UserRoleRepository : IUserRoleRepository
    {
        #region Constructor
        private readonly HRMContext _context;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        public UserRoleRepository(HRMContext context,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        #endregion

        public IQueryable<UserRole> GetUserRolesQuery()
        {
            return _context.UserRoles.IgnoreQueryFilters()
                                     .Include(r => r.Role)
                                     .Include(u => u.User)
                                     .Include(u => u.User.Department)
                                     .AsQueryable();
        }


        public List<DisplayUserRolesVM> GetUserRoles(string area)
        {
            var context = GetUserRolesQuery();
            var userRoles = (from item in context
                             where (item.User.Department.Area == area)
                             orderby item.IsActived descending, item.RegisterDate descending
                             select new DisplayUserRolesVM
                             {
                                 UserRoleId = item.UserRoleId,
                                 UserName = item.User.UserName,
                                 FirstName = item.User.FirstName,
                                 LastName = item.User.LastName,
                                 Title = item.Role.Title,
                                 Area = (item.User.Department.Area == "0" ? "استان" : (item.User.Department.Area == "1" ? "شهرستان" : "بخش")),
                                 County = (item.User.Department.County),
                                 District = (item.User.Department.District),
                                 IsActived = (item.IsActived) ? "فعال" : "غیرفعال",
                                 RegisterDate = $"{item.RegisterDate.ToShamsi()}"
                             })
                             .ToList();
            return userRoles;
        }

        public DisplayDetailsVM GetUserDetails(Guid userId)
        {
            var user = _context.Users.Where(u => u.UserId == userId)
                                     .Select(u => new DisplayDetailsVM
                                     {
                                         UserName = u.UserName,
                                         AreaDepartment = (u.Department.Area == "0" ? "استان" : (u.Department.Area == "1" ? "شهرستان" : "بخش")),
                                         ProvinceDepartment = u.Department.Province,
                                         CountyDepartment = u.Department.County,
                                         DistrictDepartment = u.Department.District,
                                         RoleTitle = u.UserRoles.Select(u => u.Role.Title).ToList(),
                                         FullName = $"{u.FirstName} {u.LastName}",
                                         LastActived = $"{u.LastActived.ToShamsi()}"
                                     })
                                     .FirstOrDefault();
            return user;
        }


        public List<SelectListItem> GetRolesForSelectBox()
        {
            var context = _roleRepository.GetRolesQuery();

            var roles = (from item in context
                         where item.IsActived
                         orderby item.RegisterDate descending
                         select new SelectListItem()
                         {
                             Text = item.Title,
                             Value = $"{item.RoleId}"
                         })
                         .ToList();
            return roles;
        }

        public List<SelectListItem> GetUsersForSelectBox(UserRolesDirectionVM direction)
        {
            var users = (from item in _context.Users
                         .Include(d => d.Department)
                         where (item.IsActived &&
                                item.Department.District == direction.District &&
                                item.Department.County == direction.County &&
                                item.Department.Area == direction.Level)
                         orderby item.RegisterDate descending
                         select new SelectListItem()
                         {
                             Value = $"{item.UserId}",
                             Text = $"{item.UserName} - {item.FirstName} {item.LastName}"
                         })
                         .ToList();
            return users;
        }

        public bool RegisterUserRole(UserRoleRegisterVM model, out string message)
        {
            string checkMessage = "";
            if (!Similarity(model, out checkMessage))
            {
                model.RegisterDate = DateTime.Now;
                model.IsActived = true;
                model.UserRoleId = Guid.NewGuid();

                UploadRegisterUserRoleToDb(model);

                message = "";
                return true;
            }
            message = checkMessage;
            return false;
        }

        public bool EditUserRole(UserRoleEditVM model, out string message)
        {
            string checkMessage = "";

            UploadEditUserRoleToDb(model);

            message = "";
            return true;


            message = checkMessage;
            return false;
        }

        public bool DisableUserRole(UserRoleEdit_Active_DisableVM model, out string message)
        {
            string checkMessage = "عملیات غیر فعال سازی با شکست مواجه شد.";

            if (model != null)
            {

                DisableUserRoleDb(model);

                message = "";

                return true;

            }
            message = checkMessage;
            return false;
        }

        public bool ActiveUserRole(UserRoleEdit_Active_DisableVM model, out string message)
        {
            string checkMessage = "عملیات فعال سازی با شکست مواجه شد.";

            if (model != null)
            {
                ActiveUserRoleDb(model);

                message = "";
                return true;
            }

            message = checkMessage;
            return false;
        }

        public UserRoleEditVM? GetUserRoleById(Guid userRoleId)
        {
            UserRoleEditVM userRole = new();
            if (userRoleId != Guid.Empty)
            {
                userRole = (from item in _context.UserRoles
                            where (item.UserRoleId == userRoleId)
                            select new UserRoleEditVM
                            {
                                UserRoleId = item.UserRoleId,
                                Title = item.Role.Title,
                                UserId = $"{item.UserId}",
                                FullName = $"{item.User.FirstName} {item.User.LastName}",
                                RoleId = $"{item.RoleId}"
                            }).Single();
            }
            return userRole;
        }

        public bool Similarity(UserRoleRegisterVM model, out string message)
        {
            bool check = false;
            var resultMessage = "";
            if (model != null)
            {
                check = _context.UserRoles.IgnoreQueryFilters()
                                          .Where(ur => ur.UserId == new Guid(model.UserId) &&
                                                       ur.RoleId == new Guid(model.RoleId))
                                          .Any();
                if (check)
                {
                    resultMessage = $"دسترسی با همین عنوان معاونت، قبلا برای کاربر ثبت شده است.";
                }
            }
            message = resultMessage;
            return check;
        }

        public void DisableUserRoleDb(UserRoleEdit_Active_DisableVM model)
        {
            if (model != null)
            {
                var userRole = _context.UserRoles
                                       .Find(model.UserRoleId);

                if (userRole != null)
                {
                    userRole.IsActived = false;
                    userRole.RegisterDate = DateTime.Now;

                    _context.UserRoles.Update(userRole);
                }
            }
        }

        public void ActiveUserRoleDb(UserRoleEdit_Active_DisableVM model)
        {
            if (model != null)
            {
                var userRole = _context.UserRoles.IgnoreQueryFilters()
                                                 .Where(ur => ur.UserRoleId == model.UserRoleId)
                                                 .FirstOrDefault();

                if (userRole != null)
                {
                    userRole.IsActived = true;
                    userRole.RegisterDate = DateTime.Now;

                    _context.UserRoles.Update(userRole);
                }
            }
        }




        public void UploadEditUserRoleToDb(UserRoleEditVM model)
        {
            if (model != null)
            {
                UserRole initial = _context.UserRoles
                                           .Find(model.UserRoleId);
                if (initial != null)
                {
                    UserRole userRole = _mapper.Map<UserRole>(model);

                    initial.RoleId = new Guid(model.RoleId);
                    initial.UserId = new Guid(model.UserId);

                    initial.RegisterDate = userRole.RegisterDate;

                    _context.Update(initial);
                }

            }
        }

        public void UploadRegisterUserRoleToDb(UserRoleRegisterVM model)
        {
            if (model != null)
            {
                UserRole userRole = _mapper.Map<UserRole>(model);

                userRole.RoleId = new Guid(model.RoleId);
                userRole.UserId = new Guid(model.UserId);

                _context.Add(userRole);
            }
        }

        public bool GetUserRoleByUserId(Guid userId)
                => _context.UserRoles.Where(ur => ur.UserId == userId &&
                                                 (ur.Role.Title == "مدیریت" ||
                                                  ur.Role.Title == "فناوری اطلاعات"))
                                     .Any();

        


        public void SaveChanges()
        {
            _context.SaveChanges();
        }


    }


}
