using Domain.DTOs.General;
using Domain.DTOs.Security.Role;
using Domain.DTOs.Security.User;
using Domain.DTOs.Security.UserRole;
using Domain.Entities.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRoleRepository
    {
        #region Role
        IQueryable<Role> GetRolesQuery();
        List<DisplayRolesVM> GetRoles();
        bool RegisterRole(RoleRegisterVM model, out string message);
        bool EditRole(RoleEditVM model, out string message);
        bool DisableRole(RoleEdit_Active_DisableVM model, out string message);
        bool ActiveRole(RoleEdit_Active_DisableVM model, out string message);
        RoleEditVM? GetRoleById(Guid roleId);
        bool Similarity(RoleRegisterVM model, out string message);
        bool Similarity(RoleEditVM model, out string message);
        #endregion



        #region DB
        void DisableRoleDb(RoleEdit_Active_DisableVM model);
        void ActiveRoleDb(RoleEdit_Active_DisableVM model);
        void UploadEditRoleToDb(RoleEditVM model);
        void UploadRegisterRoleToDb(RoleRegisterVM model);
        void SaveChanges();
        #endregion
    }
}
