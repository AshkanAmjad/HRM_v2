using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.User;
using Domain.Entities.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        #region User
        Task<User?> GetUserAsync(LoginVM loginVM);
        bool Similarity(UserRegisterVM user, out string message);
        bool Register(UserRegisterVM user, out string message);
        List<DisplayUsersVM> GetUsers(AreaVM area);
        IQueryable<User> GetUsersQuery();
        UserEditVM? GetUserById(Guid userId, AreaVM area);
        #endregion

        #region DB
        void UploadUserToDb(UserRegisterVM user);
        void UploadDepartmentToDb(UserRegisterVM depatment, Guid departmentId);
        void SaveChanges();
        #endregion

    }
}
