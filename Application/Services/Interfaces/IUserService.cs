using Domain.DTOs.Security.Login;
using Domain.Entities.Security.Models;
using System.Web.Mvc;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUser(LoginVM loginDTO);
        List<SelectListItem> GetAreas();
    }
}
