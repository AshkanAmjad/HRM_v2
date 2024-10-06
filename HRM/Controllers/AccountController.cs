using Application.Extensions;
using Application.Services.Interfaces;
using Domain.DTOs.Security.Login;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using HRM.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using System.Security.Claims;

namespace HRM.Controllers
{
    public class AccountController : Controller
    {
        #region Constructor
        private readonly IUserRepository _userRepository;
        private readonly IValidator<LoginVM> _validator;


        public AccountController(IUserService userService, IValidator<LoginVM> validator, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _validator = validator;

        }
        #endregion

        #region Select lists
        public List<SelectListItem> Areas()
        {
            List<SelectListItem> areas = new()
            {
                new SelectListItem
                {
                    Text = "استان",
                    Value = 0.ToString()
                },
                new SelectListItem
                {
                    Text = "شهرستان",
                    Value = 1.ToString()
                },
                new SelectListItem
                {
                    Text = "بخش",
                    Value = 3.ToString()
                }
            };
            return areas;
        }
        #endregion
        public IActionResult Login()
        {
            var areas = Areas();
            ViewData["Areas"] = areas;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginVM model)
        {
            ValidationResult result = _validator.Validate(model);

            if (result.IsValid)
            {
                var verify = false;

                var user = _userRepository.GetUser(model);

                if (user != null)
                {
                    verify = Hashing.Verify(user.Password, model.Password);
                }

                if (!verify)
                {
                    ModelState.AddModelError("UserName", "کاربری با مشخصات وارد شده یافت نشد");
                }
                else if (verify)
                {
                    var claims = new List<Claim>
                    {
                        new ("userId",user.UserId.ToString())
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };
                    HttpContext.SignInAsync(principal, properties);

                    ViewData["isSucess"] = true;
                    ViewData["area"] = model.Area;
                    return View(model);
                }

                #region Error Messages
                if (user is null)
                {
                    ModelState.AddModelError("UserName", "کاربری با مشخصات وارد شده یافت نشد");
                }
                if (user != null && !user.IsActived)
                {
                    ModelState.AddModelError("UserName", "حساب کاربری غیر فعال است.");
                }
                #endregion
            }
            #region Manual Validation
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            result.AddToModelState(this.ModelState);
            #endregion

            #region Areas
            var areas = Areas();
            ViewData["Areas"] = areas;
            #endregion

            return View();
        }
    }
}
