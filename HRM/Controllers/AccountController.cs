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
using System.Linq.Expressions;
using System.Security.Claims;
using System.Web.WebPages.Html;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            ValidationResult result = _validator.Validate(model);

            if (result.IsValid)
            {

                #region hashing password
                var hasher = new PasswordHasher<string>();
                var password = model.Password;
                var hashedPassword = hasher.HashPassword(null, password);
                #endregion

                var user = await _userService.GetUser(model);
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
                    await HttpContext.SignInAsync(null, principal);
                    return View("Index", model);
                }

                #region Error
                if (user is null)
                {
                    ModelState.AddModelError("UserName", "کاربری با مشخصات وارد شده یافت نشد");
                }
                if (user.IsActived == false)
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
            var areas = new HomeController().Areas();
            ViewData["Areas"] = areas;
            #endregion

            return View("Index", model);
        }
    }
}
