using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.User;
using Domain.Entities.Security.Models;
using System.Web.Mvc;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        #region User
        Task<User?> GetUser(LoginVM model);
        List<SelectListItem> GetAreas();
        bool Register(UserRegisterVM model, out string message);
        string Hashing(string password);
        #endregion
    }
}
