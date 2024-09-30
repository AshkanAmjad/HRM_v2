using AutoMapper;
using Data.Context;
using Data.Extensions;
using Domain.DTOs.General;
using Domain.DTOs.Security.UserRole;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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


        public List<DisplayUserRolesVM> GetUserRoles()
        {
            var context = GetUserRolesQuery();
            var userRoles = (from item in context
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




    }


}
