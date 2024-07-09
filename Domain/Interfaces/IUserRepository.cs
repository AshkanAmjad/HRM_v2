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
        Task<User?>GetUser(LoginVM loginVM);
        bool Similarity(UserRegisterVM userRegister, out string message);
        bool Register(UserRegisterVM userRegister, out string message);
        #endregion

    }
}
