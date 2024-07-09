using Application.Services.Interfaces;
using Domain.DTOs.Security.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]

    public class ManagementController : Controller
    {
        #region Constructor
        private readonly IUserService _userService;
        private readonly IValidator<UserRegisterVM> _userRegisterValidator;


        public ManagementController(IUserService userService, IValidator<UserRegisterVM> userRegisterValidator)
        {
            _userService = userService;
            _userRegisterValidator = userRegisterValidator;
        }
        #endregion

        #region Select lists
        public List<SelectListItem> GenderTypes()
        {
            List<SelectListItem> genders = new()
            {
                new SelectListItem
                {
                    Text = "مرد",
                    Value = "M"
                },
                new SelectListItem
                {
                    Text = "زن",
                    Value = "W"
                }
            };
            return genders;
        }

        public List<SelectListItem> MariltalTypes()
        {
            List<SelectListItem> genders = new()
            {
                new SelectListItem
                {
                    Text = "مجرد",
                    Value = "S"
                },
                new SelectListItem
                {
                    Text = "متاهل",
                    Value = "M"
                }
            };
            return genders;
        }

        public List<SelectListItem> EmploymentTypes()
        {
            List<SelectListItem> employment = new()
            {
                new SelectListItem
                {
                    Text = "آزمایشی",
                    Value = "T"
                },
                new SelectListItem
                {
                    Text = "قراردادی",
                    Value = "C"
                },
                new SelectListItem
                {
                    Text = "رسمی",
                    Value = "O"
                },
            };
            return employment;
        }
        #endregion

        public IActionResult ProvinceIndex()
        {
            return View();
        }

        public IActionResult Register()
        {
            var genders = GenderTypes();
            var marital = MariltalTypes();
            var employment = EmploymentTypes();
            ViewData["Gendes"] = genders;
            ViewData["Marital"] = marital;
            ViewData["Employment"] = employment;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterVM user )
        {
            ValidationResult userValidator = _userRegisterValidator.Validate(user);
            bool success = false;
            var message = $"عملیات ثبت با شکست مواجه شده است.";
            string checkMessage = "";
            if (userValidator.IsValid)
            {
                try
                {
                    bool result = _userService.Register(user, out checkMessage);
                    if (result)
                    {
                        success = true;
                        message = $"<h5>عملیات ثبت کاربر <span class='text-primary'> {user.FirstName}  {user.LastName} </span> با موفقیت انجام شد.</h5>";
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
                    message = $"<h5>خطای شکست عملیات ثبت : {ex.Message} </h5>";
                }
            }
            else
            {
                message = $"{userValidator}";
            }
            #region Manual Validation
            foreach (var error in userValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            userValidator.AddToModelState(this.ModelState);
            #endregion

            #region Json data
            var data = new
            {
                success = success,
                message = message,
            };
            #endregion

            return Json(data);
        }


        public IActionResult Profile()
        {
            return View();
        }
    }
}
