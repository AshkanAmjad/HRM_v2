﻿using Domain.DTOs.General;
using Domain.DTOs.Security.Role;
using Domain.DTOs.Security.User;
using Domain.DTOs.Security.UserRole;
using Domain.Entities.Security.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Domain.Interfaces
{
    public interface IUserRoleRepository
    {
        #region UserRole
        IQueryable<UserRole> GetUserRolesQuery(AreaVM area);
        List<DisplayUserRolesVM> GetUserRoles(AreaVM area);
        Task<DisplayDetailsVM> GetUserDetailsAsync(Guid userId);
        List<string> GetUserRolesByUserId(Guid userId);
        List<SelectListItem> GetRolesForSelectBox();
        List<SelectListItem> GetUsersForSelectBox(UserRolesDirectionVM direction);
        bool RegisterUserRole(UserRoleRegisterVM model, out string message);
        bool EditUserRole(UserRoleEditVM model, out string message);
        bool DisableUserRole(UserRoleEdit_Active_DisableVM model, out string message);
        bool ActiveUserRole(UserRoleEdit_Active_DisableVM model, out string message);
        UserRoleEditVM? GetUserRoleById(Guid userRoleId);
        bool IsValidUserRoleByUserId(Guid userId);
        List<string> GetUserRoleByUserId(Guid userId);
        bool Similarity(UserRoleRegisterVM model, out string message);
        bool Similarity(UserRoleEditVM model, out string message);
        #endregion

        #region DB
        void DisableUserRoleDb(UserRoleEdit_Active_DisableVM model);
        void ActiveUserRoleDb(UserRoleEdit_Active_DisableVM model);
        void UploadEditUserRoleToDb(UserRoleEditVM model);
        void UploadRegisterUserRoleToDb(UserRoleRegisterVM model);
        void SaveChanges();
        #endregion
    }
}
