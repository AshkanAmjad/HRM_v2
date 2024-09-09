using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.User;
using Domain.Entities.Security.Models;
using Microsoft.AspNetCore.Http;
using System.Web.Mvc;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        #region User
        List<SelectListItem> GetAreas();
        bool Register(UserRegisterVM model, out string message);
        bool Edit(UserEditVM model, out string message);
        bool Disable(UserEdit_DisableVM model, out string message);
        List<DisplayUsersVM> GetUsers(AreaVM area);
        #endregion


    }
}
