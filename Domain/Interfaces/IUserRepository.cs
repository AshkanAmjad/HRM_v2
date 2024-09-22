using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.User;
using Domain.Entities.Portal.Models;
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
        bool Edit(UserEditVM user, out string message);
        bool Delete(UserDelete_ActiveVM user, out string message);
        bool Disable(UserEdit_DisableVM model, out string message);
        bool Active(UserDelete_ActiveVM model, out string message);
        List<DisplayUsersVM> GetUsers(AreaVM area);
        List<DisplayUsersVM> GetArchivedUsers(AreaVM area);
        IQueryable<User> GetUsersQuery();
        UserEditVM? GetUserById(Guid userId);
        Guid? GetDepartmentIdByUserId(Guid userId, string area);
        #endregion

        #region DB
        void UploadRegisterUserToDb(UserRegisterVM userRegister);
        void DisableUser(UserEdit_DisableVM model);
        void ActiveUser(UserDelete_ActiveVM model);
        void DisableDepartment(UserEdit_DisableVM model , out Guid departmentId);
        void ActiveDepartment(UserDelete_ActiveVM model , out Guid departmentId);
        void UploadEditUserToDb(UserEditVM userEdit);
        void UploadDepartmentToDb(UserRegisterVM depatment, Guid departmentId);
        void UploadEditDepartmentToDb(UserEditVM model);
        void DeleteAvatarOnDb(Guid departmentId);
        void SaveChanges();
        #endregion

        #region Is Exist
        bool IsExistAvatar(Guid departmentId);
        #endregion
    }
}
