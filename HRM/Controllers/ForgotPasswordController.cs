using Application.Services.Interfaces;
using Domain.DTOs.Security.Login;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using HRM.Models.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    public class ForgotPasswordController : Controller
    {

        #region Constructor
        private readonly IUserService _userService;
        private readonly IGeneralService _generalService;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UsernameValidationVM> _userNameValidationvalidator;


        public ForgotPasswordController(IUserService userService,
                                        IUserRepository userRepository,
                                        IGeneralService generalService,
                                        IValidator<UsernameValidationVM> userNameValidationvalidator
                                        )
        {
            _userService = userService;
            _userNameValidationvalidator = userNameValidationvalidator;
            _generalService = generalService;
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
            var areas = _generalService.Areas();
            ViewData["Areas"] = areas;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UsernameValidation(UsernameValidationVM model)
        {
            ValidationResult userNameValidator = _userNameValidationvalidator.Validate(model);
            bool success = false;
            var message = $"عملیات اعتبار سنجی با شکست مواجه شده است.";
            string checkMessage = "";

            if (userNameValidator.IsValid)
            {
                try
                {
                    //bool result = _userRepository.Register(user, out checkMessage);

                    //if (result)
                    //{
                    //    _userRepository.SaveChanges();
                    //    success = true;
                    //    message = $"<h5>ایمیل بازیابی رمز عبور با موفقیت ارسال شد.</h5>";
                    //}
                    //else
                    //{
                    //    message = checkMessage;
                    //}
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    message = $"خطای شکست عملیات  : {ex.Message}";
                }
            }
            else
            {
                message = $"{userNameValidator}";
            }
            #region Manual Validation
            foreach (var error in userNameValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            userNameValidator.AddToModelState(this.ModelState);
            #endregion

            #region Json data
            var jsonData = new
            {
                success = success,
                message = message,
            };
            #endregion

            return Json(jsonData);
        }
        #endregion

    }
}
