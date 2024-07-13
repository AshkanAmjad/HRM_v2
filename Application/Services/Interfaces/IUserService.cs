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
        Task<User?> GetUserAsync(LoginVM model);
        List<SelectListItem> GetAreas();
        bool Register(UserRegisterVM model, out string message);
        string Hashing(string password);
        Task<List<DisplayUsersVM>> GetUsersAsync(AreaVM area);
        #endregion

        #region Document
        public void UploadDocumentToServer(UploadVM document);
        public DirectionVM UploadDirectionOnServer(DirectionVM direction);
        #endregion
    }
}
