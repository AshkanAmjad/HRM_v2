﻿using Application.Extensions;
using Application.Services.Interfaces;
using Domain.DTOs.Security.Login;
using Domain.Entities.Security.Models;
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
        private readonly IGeneralService _generalService;
        private readonly IValidator<LoginVM> _validator;


        public AccountController(IUserService userService, IGeneralService generalService, IValidator<LoginVM> validator, IUserRepository userRepository)
        {

            _userRepository = userRepository;
            _generalService = generalService;
            _validator = validator;

        }
        #endregion


        #region Login
        public IActionResult Login()
        {
            var areas = _generalService.Areas();
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

                    _userRepository.RecordActivity(user.UserId);
                    _userRepository.SaveChanges();

                    ViewData["IsSuccessLogin"] = true;
                    ViewData["Area"] = model.Area;

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
            var areas = _generalService.Areas();
            ViewData["Areas"] = areas;
            #endregion



            return View();
        }
        #endregion

        #region Logout

        public IActionResult Logout()
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == "userId").Value;
            var userId = new Guid(id);

            _userRepository.RecordActivity(userId);
            _userRepository.SaveChanges();

            HttpContext.SignOutAsync();
            TempData["IsSuccessLogout"] = true;

            return RedirectToAction("Login", "Account");
        }
        #endregion
    }
}
