using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.User;
using Domain.Entities.Security.Models;
using System.Web.Mvc;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUser(LoginVM model);
        List<SelectListItem> GetAreas();
        Task<bool> Register(UserRegisterVM model);
    }
}
