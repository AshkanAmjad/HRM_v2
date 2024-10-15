using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.Profile;
using Domain.DTOs.Security.User;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        #region User
        List<SelectListItem> GetAreas();
        bool Register(UserRegisterVM model, out string message);
        bool Edit(UserEditVM model, out string message);
        bool Disable(UserEdit_DisableVM model, out string message);
        bool Active(UserDelete_ActiveVM model, out string message);
        #endregion

        #region Profile
        bool Edit(ProfileEditVM model, out string message);
        #endregion

        #region Login
        bool VerificationCode(VerificationCodeVM model, string code, out string message);
        bool ResetPassword(ResetPasswordVM model, out string message);
        #endregion


    }
}
