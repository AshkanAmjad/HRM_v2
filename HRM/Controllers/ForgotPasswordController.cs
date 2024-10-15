using Application.Extensions;
using Application.Senders;
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
        private readonly IValidator<VerificationCodeVM> _verificationValidator;
        private readonly IValidator<ResetPasswordVM> _resetPasswordValidator;
        private readonly IViewRenderService _viewRenderService;


        public ForgotPasswordController(IUserService userService,
                                        IUserRepository userRepository,
                                        IGeneralService generalService,
                                        IViewRenderService viewRenderService,
                                        IValidator<UsernameValidationVM> userNameValidationvalidator,
                                        IValidator<VerificationCodeVM> verificationValidator,
                                        IValidator<ResetPasswordVM> resetPasswordValidator
                                        )
        {
            _userService = userService;
            _userNameValidationvalidator = userNameValidationvalidator;
            _verificationValidator = verificationValidator;
            _generalService = generalService;
            _userRepository = userRepository;
            _viewRenderService = viewRenderService;
            _resetPasswordValidator=resetPasswordValidator;
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
            Guid checkUserId = Guid.Empty;

            if (userNameValidator.IsValid)
            {
                try
                {
                    bool result = _userRepository.UserNameValidation(model, out checkUserId, out checkMessage);

                    if (result)
                    {
                        message = checkMessage;
                        TempData["UserId"] = checkUserId;
                        success = true;
                    }
                    else
                    {
                        message = checkMessage;
                    }
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

        #region ChooseTheMethod
        public IActionResult ChooseTheMethod()
        {
            return View();
        }
        #endregion

        #region Sending verification by email
        public IActionResult SendVerificationCodeByEmail()
        {

            string checkMessage = "";

            var id = TempData["UserId"];
            TempData.Keep("UserId");


            if (id == null)
            {
                return NotFound();
            }

            var userId = (Guid)id;
            string email = _userRepository.GetEmailByUserId(userId);

            string code = NumberGenerator.RandomNumber();

            TempData["Code"] = code;

            if (code == "")
            {
                return NotFound();
            }

            string bodyEmail = _viewRenderService.RenderToStringAsync("_EmailTemplate", code);

            bool result = SendEmail.Send(email, "بازیابی گذر واژه حساب کاربری", bodyEmail , out checkMessage);

            #region Json data
            var jsonData = new
            {
                success = result,
                message=checkMessage,
            };
            #endregion

            return Json(jsonData);
        }
        #endregion

        #region Verification
        public IActionResult VerificationCode()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerificationCode(VerificationCodeVM model)
        {
            ValidationResult validate = _verificationValidator.Validate(model);
            bool success = false;
            var message = $"عملیات اعتبار سنجی با شکست مواجه شده است.";

            if (validate.IsValid)
            {
                try
                {
                    var code = (string)TempData["Code"];

                    bool result = _userService.VerificationCode(model, code, out message);

                    if(result)
                    {
                        TempData.Remove("Code");
                    }
                    else 
                    {
                        TempData.Keep("Code");
                    }

                    success = result;
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
                message = $"{validate}";
            }

            #region Manual Validation
            validate.AddToModelState(this.ModelState);
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

        #region Reset password
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPasswordVM model)
        {
            ValidationResult resetValidator = _resetPasswordValidator.Validate(model);
            bool success = false;
            var message = $"عملیات باز نشانی گذر واژه با شکست مواجه شده است.";
            string checkMessage = "";
            if (resetValidator.IsValid)
            {
                try
                {
                    model.UserId = (Guid)TempData["UserId"];

                    bool result = _userService.ResetPassword(model, out checkMessage);

                    if (result)
                    {
                        _userRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات باز نشانی گذر واژه، با موفقیت انجام شد.</h5>";
                        TempData.Remove("UserId");
                    }
                    else
                    {
                        message = checkMessage;
                        TempData.Keep("UserId");
                    }
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
                message = $"{resetValidator}";
            }

            #region Manual Validation
            foreach (var error in resetValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            resetValidator.AddToModelState(this.ModelState);
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

        #region Verification by phone number
        public IActionResult SendVerificationCodeByPhoneNumber()
        {
            return View();
        }
        #endregion


    }
}
