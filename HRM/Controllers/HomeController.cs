using Application.Services.Interfaces;
using Domain.DTOs.Security.Login;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using HRM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace HRM.Controllers
{
    public class HomeController : Controller
    {
        #region Constructor
        private readonly IManagementServices _managementServices;
        private readonly IValidator<LoginViewModel> _validator;

        public HomeController(IManagementServices managementServices, IValidator<LoginViewModel> validator)
        {
            _managementServices = managementServices;
            _validator = validator;
        }

        #endregion
        List<SelectListItem> Areas()
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

        public IActionResult Index()
        {
            var areas = Areas();
            ViewData["Areas"] = areas;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            ValidationResult result=_validator.Validate(model);
            if (result.IsValid)
            {
            
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

            return View("Index",model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
