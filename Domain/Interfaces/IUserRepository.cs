using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.Profile;
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
        bool Similarity(UserRegisterVM user, out string message);
        bool Register(UserRegisterVM user, out string message);
        bool Edit(UserEditVM user, out string message);
        bool Delete(UserDelete_ActiveVM user, out string message);
        bool Disable(UserEdit_DisableVM model, out string message);
        bool Active(UserDelete_ActiveVM model, out string message);
        bool ResetPassword(ResetPasswordVM model, out string message);
        List<DisplayUsersVM> GetUsers(AreaVM area);
        List<DisplayUsersVM> GetArchivedUsers(AreaVM area);
        IQueryable<User> GetUsersQuery();
        UserEditVM? GetUserById(Guid userId);
        Guid? GetDepartmentIdByUserId(Guid userId, string area);
        #endregion

        #region Login
        User? GetUser(LoginVM loginVM);
        bool UserNameValidation(UsernameValidationVM model, out Guid userId,out string message);
        void GetEmailAndPhoneNumberByUserId(Guid userId, out string phoneNumber, out string email);
        string GetEmailByUserId(Guid userId);
        #endregion

        #region Profile
        DisplayProfileVM GetProfileById(Guid userId);
        ProfileEditVM? GetUserProfileById(Guid userId);
        bool Edit(ProfileEditVM user, out string message);
        #endregion

        #region DB
        void UploadRegisterUserToDb(UserRegisterVM userRegister);
        void DisableUserDb(UserEdit_DisableVM model);
        void ActiveUserDb(UserDelete_ActiveVM model);
        void DisableDepartmentDb(UserEdit_DisableVM model, out Guid departmentId);
        void ActiveDepartmentDb(UserDelete_ActiveVM model, out Guid departmentId);
        void UploadEditUserToDb(UserEditVM userEdit);
        void UploadDepartmentToDb(UserRegisterVM depatment, Guid departmentId);
        void UploadEditDepartmentToDb(UserEditVM model);
        void DeleteAvatarOnDb(Guid departmentId);
        void UploadEditUserToDb(ProfileEditVM userEdit);
        void UploadResetPasswordToDb(ResetPasswordVM model);
        void RecordActivity(Guid userId);
        void SaveChanges();
        #endregion

        #region Is Exist
        bool IsExistAvatar(Guid departmentId);
        #endregion
    }
}
