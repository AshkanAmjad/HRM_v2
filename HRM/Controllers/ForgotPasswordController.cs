using Application.Services.Interfaces;
using Domain.DTOs.Security.Login;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    public class ForgotPasswordController : Controller
    {

        #region Constructor
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;


        public ForgotPasswordController(IUserService userService,
                                        IUserRepository userRepository
                                        )
        {
            _userService = userService;
            _userRepository = userRepository;
        }
        #endregion

        #region Index
        public IActionResult ForgotPasswordIndex()
        {
            return View();
        }
        #endregion

        #region Username Validation
        public IActionResult UsernameValidation()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult UsernameValidation()
        //{
        //    return View();
        //}
        #endregion

    }
}
