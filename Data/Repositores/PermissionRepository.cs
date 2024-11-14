using AutoMapper;
using Data.Context;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositores
{
    public class PermissionRepository : IPermissionRepository
    {
        #region Constructor
        private readonly HRMContext _context;
        private readonly IMapper _mapper;
        private readonly IUserRoleRepository _userRoleRepository;
        public PermissionRepository(HRMContext context,
            IMapper mapper,
            IUserRoleRepository userRoleRepository)
        {
            _context = context;
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
        }
        #endregion
        public bool CheckAreaPermission(string area, Guid userId)
        {
            var result = false;
            if (userId != Guid.Empty)
            {
                result = _context.Departments.AsNoTracking()
                                             .Where(d => d.UserId == userId && d.Area == area).Any();
            }
            return result;
        }

        public bool CheckRolePermission(string[] roleTitles, Guid userId)
        {
                var roles = GetUserRolesById(userId);

                if (roles.Count == 0)
                    return false;

                foreach (var role in roles)
                {
                    if (roleTitles.Contains(role))
                        return true;
                }

            return false;
        }

        public List<string> GetUserRolesById(Guid userId)
        {
            List<string> userRoles = new();

            if (userId != Guid.Empty)
            {

                userRoles = (from item in _context.UserRoles.AsNoTracking()
                             where (item.UserId == userId)
                             select item.Role.Title)
                             .ToList();
            }
            return userRoles;
        }
    }
}
