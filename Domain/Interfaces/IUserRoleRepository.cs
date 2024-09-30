using Domain.DTOs.General;
using Domain.DTOs.Security.UserRole;
using Domain.Entities.Security.Models;
using System.Web.Mvc;

namespace Domain.Interfaces
{
    public interface IUserRoleRepository
    {
        #region UserRole
        IQueryable<UserRole> GetUserRolesQuery();
        List<DisplayUserRolesVM> GetUserRoles();
        List<SelectListItem> GetRolesForSelectBox();
        List<SelectListItem> GetUsersForSelectBox(UserRolesDirectionVM direction);

        #endregion
    }
}
