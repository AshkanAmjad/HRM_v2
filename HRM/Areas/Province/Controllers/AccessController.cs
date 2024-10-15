using Application.Services.Interfaces;
using AutoMapper;
using Data.Extensions;
using Domain.DTOs.General;
using Domain.DTOs.Security.Role;
using Domain.DTOs.Security.UserRole;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using HRM.Models.Validation.Security.UserRole;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]
    [AreaPermissionChecker("0")]
    [RolePermissionChecker("مدیریت", "فناوری اطلاعات")]

    public class AccessController : Controller
    {
        #region Constructor
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IGeneralService _generalService;
        private readonly IMapper _mapper;
        private readonly IValidator<RoleRegisterVM> _roleRegisterValidator;
        private readonly IValidator<RoleEditVM> _roleEditValidator;
        private readonly IValidator<RoleEdit_Active_DisableVM> _roleEdit_DisableValidator;
        private readonly IValidator<UserRoleRegisterVM> _userRoleRegisterValidator;
        private readonly IValidator<UserRoleEditVM> _userRoleEditValidator;
        private readonly IValidator<UserRoleEdit_Active_DisableVM> _userRoleEdit_DisableValidator;



        public AccessController(
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            IMapper mapper,
            IGeneralService generalService,
            IValidator<RoleRegisterVM> roleRegisterValidator,
            IValidator<RoleEditVM> roleEditValidator,
            IValidator<RoleEdit_Active_DisableVM> roleEdit_DisableValidator,
            IValidator<UserRoleRegisterVM> userRoleRegisterValidator,
            IValidator<UserRoleEditVM> userRoleEditValidator,
            IValidator<UserRoleEdit_Active_DisableVM> userRoleEdit_DisableValidator)


        {
            _roleRepository = roleRepository;
            _generalService = generalService;
            _roleEditValidator = roleEditValidator;
            _roleRegisterValidator = roleRegisterValidator;
            _roleEdit_DisableValidator = roleEdit_DisableValidator;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _userRoleRegisterValidator = userRoleRegisterValidator;
            _userRoleEditValidator = userRoleEditValidator;
           _userRoleEdit_DisableValidator = userRoleEdit_DisableValidator;
        }
        #endregion

        #region Index
        public IActionResult ProvinceRolesIndex()
        {
            return View();
        }

        public IActionResult ProvinceUserRolesIndex()
        {
            return View();
        }

        #endregion

        #region Display
        public IActionResult FillRolesGrid()
        {
            return View();
        }

        public IActionResult FillUserRolesGrid()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetRoles()
        {
            var roles = _roleRepository.GetRoles();

            #region paging and searching
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";


            var mainData = roles
                .Where(a => a.Title.Contains(searchValue))
                .Skip(start)
                .Take(length)
                .ToList();

            var totalCount = roles
                .Count();

            #endregion

            var jsonData = new
            {
                draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0"),
                recordTotal = totalCount,
                recordsFiltered = mainData.Count(),
                data = mainData
            };

            return Json(jsonData);
        }

        [HttpPost]
        public IActionResult GetUserRoles(string section)
        {
            var roles = _userRoleRepository.GetUserRoles(section);

            #region paging and searching
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";


            var mainData = roles
                .Where(a => (a.Title.Contains(searchValue)) ||
                      (a.UserName.Contains(searchValue)))
                .Skip(start)
                .Take(length)
                .ToList();

            var totalCount = roles
                .Count();

            #endregion

            var jsonData = new
            {
                draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0"),
                recordTotal = totalCount,
                recordsFiltered = mainData.Count(),
                data = mainData
            };

            return Json(jsonData);
        }
        #endregion

        #region Register
        public IActionResult RoleRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RoleRegister(RoleRegisterVM model)
        {
            ValidationResult roleValidator = _roleRegisterValidator.Validate(model);
            bool success = false;
            var message = $"عملیات ثبت با شکست مواجه شده است.";
            string checkMessage = "";
            if (roleValidator.IsValid)
            {
                try
                {
                    bool result = _roleRepository.RegisterRole(model, out checkMessage);

                    if (result)
                    {
                        _roleRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات ثبت معاونت <span class='text-primary'> {model.Title} </span> با موفقیت انجام شد.</h5>";
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
                message = $"{roleValidator}";
            }
            #region Manual Validation
            foreach (var error in roleValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            roleValidator.AddToModelState(this.ModelState);
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

        public IActionResult UserRoleRegister()
        {
            var departments = _generalService.ProvinceDepartmentTypes();
            ViewBag.Departments = new SelectList(departments, "Value", "Text");

            var roles = _userRoleRepository.GetRolesForSelectBox();
            ViewBag.Roles = new SelectList(roles, "Value", "Text");


            return View();
        }

        [HttpPost]
        public IActionResult GetUsersForSelectBox(UserRolesDirectionVM direction)
        {
            var users = _userRoleRepository.GetUsersForSelectBox(direction);

            #region Json data
            var jsonData = new
            {
                data = new SelectList(users, "Value", "Text"),
                success = true
            };
            #endregion

            return Json(jsonData);
        }

        [HttpPost]
        public IActionResult UserRoleRegister(UserRoleRegisterVM model)
        {
            ValidationResult userRoleValidator = _userRoleRegisterValidator.Validate(model);
            bool success = false;
            var message = $"عملیات ثبت با شکست مواجه شده است.";
            string checkMessage = "";
            if (userRoleValidator.IsValid)
            {
                try
                {
                    bool result = _userRoleRepository.RegisterUserRole(model, out checkMessage);

                    if (result)
                    {
                        _userRoleRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات ثبت دسترسی با موفقیت انجام شد.</h5>";
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
                message = $"{userRoleValidator}";
            }
            #region Manual Validation
            foreach (var error in userRoleValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            userRoleValidator.AddToModelState(this.ModelState);
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

        #region Edit
        public IActionResult RoleEdit(RoleEdit_Active_DisableVM model)
        {
            ValidationResult roleValidator = _roleEdit_DisableValidator.Validate(model);

            if (roleValidator.IsValid)
            {
                var role = _roleRepository.GetRoleById(model.RoleId);

                if (role == null)
                    return NotFound();

                return View(role);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RoleEdit(RoleEditVM model)
        {
            ValidationResult roleValidator = _roleEditValidator.Validate(model);
            bool success = false;
            var message = $"عملیات ویرایش با شکست مواجه شده است.";
            string checkMessage = "";
            if (roleValidator.IsValid)
            {
                try
                {
                    bool result = _roleRepository.EditRole(model, out checkMessage);

                    if (result)
                    {
                        _roleRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات ویرایش معاونت <span class='text-primary'> {model.Title} </span> با موفقیت انجام شد.</h5>";
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
                message = $"{roleValidator}";
            }
            #region Manual Validation
            foreach (var error in roleValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            roleValidator.AddToModelState(this.ModelState);
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

        public IActionResult UserRoleEdit(UserRoleEdit_Active_DisableVM model)
        {
            ValidationResult userRoleValidator = _userRoleEdit_DisableValidator.Validate(model);

            if (userRoleValidator.IsValid)
            {
                var userRole = _userRoleRepository.GetUserRoleById(model.UserRoleId);

                if (userRole == null)
                    return NotFound();

                var roles = _userRoleRepository.GetRolesForSelectBox();
                ViewBag.Roles = new SelectList(roles, "Value", "Text");

                return View(userRole);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult UserRoleEdit(UserRoleEditVM model)
        {
            ValidationResult userRoleValidator = _userRoleEditValidator.Validate(model);
            bool success = false;
            var message = $"عملیات ویرایش با شکست مواجه شده است.";
            string checkMessage = "";
            if (userRoleValidator.IsValid)
            {
                try
                {
                    bool result = _userRoleRepository.EditUserRole(model, out checkMessage);

                    if (result)
                    {
                        _roleRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات ویرایش دسترسی با موفقیت انجام شد.</h5>";
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
                message = $"{userRoleValidator}";
            }
            #region Manual Validation
            foreach (var error in userRoleValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            userRoleValidator.AddToModelState(this.ModelState);
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

        #region Disable
        [HttpPost]
        public IActionResult RoleDisable(RoleEdit_Active_DisableVM model)
        {
            ValidationResult roleValidator = _roleEdit_DisableValidator.Validate(model);
            bool success = false;
            var message = $"عملیات غیر فعال سازی با شکست مواجه شده است.";
            string checkMessage = "";
            if (roleValidator.IsValid)
            {
                try
                {
                    bool result = _roleRepository.DisableRole(model, out checkMessage);

                    if (result)
                    {
                        _roleRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات غیر فعال سازی معاونت <span class='text-primary'> {model.Title} </span> با موفقیت انجام شد.</h5>";
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
                    message = $"خطای شکست عملیات  :  {ex.Message}";
                }
            }
            else
            {
                message = $"{roleValidator}";
            }
            #region Manual Validation
            foreach (var error in roleValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            roleValidator.AddToModelState(this.ModelState);
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

        [HttpPost]
        public IActionResult UserRoleDisable(UserRoleEdit_Active_DisableVM model)
        {
            ValidationResult userRoleValidator = _userRoleEdit_DisableValidator.Validate(model);
            bool success = false;
            var message = $"عملیات غیر فعال سازی با شکست مواجه شده است.";
            string checkMessage = "";
            if (userRoleValidator.IsValid)
            {
                try
                {
                    bool result = _userRoleRepository.DisableUserRole(model, out checkMessage);

                    if (result)
                    {
                        _roleRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات غیر فعال سازی دسترسی با موفقیت انجام شد.</h5>";
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
                    message = $"خطای شکست عملیات  :  {ex.Message}";
                }
            }
            else
            {
                message = $"{userRoleValidator}";
            }
            #region Manual Validation
            foreach (var error in userRoleValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            userRoleValidator.AddToModelState(this.ModelState);
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

        #region Active
        [HttpPost]
        public IActionResult RoleActive(RoleEdit_Active_DisableVM model)
        {
            ValidationResult roleValidator = _roleEdit_DisableValidator.Validate(model);
            bool success = false;
            var message = $"عملیات غیر فعال سازی با شکست مواجه شده است.";
            string checkMessage = "";
            if (roleValidator.IsValid)
            {
                try
                {
                    bool result = _roleRepository.ActiveRole(model, out checkMessage);

                    if (result)
                    {
                        _roleRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات فعال سازی معاونت <span class='text-primary'> {model.Title} </span> با موفقیت انجام شد.</h5>";
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
                    message = $"خطای شکست عملیات  :  {ex.Message}";
                }
            }
            else
            {
                message = $"{roleValidator}";
            }
            #region Manual Validation
            foreach (var error in roleValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            roleValidator.AddToModelState(this.ModelState);
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

        [HttpPost]
        public IActionResult UserRoleActive(UserRoleEdit_Active_DisableVM model)
        {
            ValidationResult userRoleValidator = _userRoleEdit_DisableValidator.Validate(model);
            bool success = false;
            var message = $"عملیات غیر فعال سازی با شکست مواجه شده است.";
            string checkMessage = "";
            if (userRoleValidator.IsValid)
            {
                try
                {
                    bool result = _userRoleRepository.ActiveUserRole(model, out checkMessage);

                    if (result)
                    {
                        _roleRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات فعال سازی دسترسی با موفقیت انجام شد.</h5>";
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
                    message = $"خطای شکست عملیات  :  {ex.Message}";
                }
            }
            else
            {
                message = $"{userRoleValidator}";
            }
            #region Manual Validation
            foreach (var error in userRoleValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            userRoleValidator.AddToModelState(this.ModelState);
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