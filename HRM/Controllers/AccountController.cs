using Application.Services.Interfaces;
using Domain.DTOs.Security.Login;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using HRM.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Security.Claims;

namespace HRM.Controllers
{
    public class AccountController : Controller
    {
        #region Constructor
        private readonly IUserService _userService;
        private readonly IValidator<LoginVM> _validator;

        public AccountController(IUserService userService, IValidator<LoginVM> validator)
        {
            _userService = userService;
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
        public async Task<IActionResult> Login(LoginVM model)
        {
            ValidationResult result = _validator.Validate(model);

            if (result.IsValid)
            {
                var user = await _userService.GetUserAsync(model);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new ("nationalCode",user.UserName.ToString()),
                        new ("firstName",user.FirstName.ToString()),
                        new ("lastName",user.LastName.ToString()),
                        new ("lastActived",user.LastActived.ToString()),
                        new ("isActived",user.IsActived.ToString())
                    };
                    var identity = new ClaimsIdentity(claims);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };
                    await HttpContext.SignInAsync(principal,properties);
                    return View(model);
                }

                #region Error Messages
                if (user is null)
                {
                    ModelState.AddModelError("UserName", "کاربری با مشخصات وارد شده یافت نشد");
                }
                if (user != null && user.IsActived == false)
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
            var areas =Areas();
            ViewData["Areas"] = areas;
            #endregion

            return View();
        }
    }
}
