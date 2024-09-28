using AutoMapper;
using Domain.DTOs.Security.Role;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]
    public class AccessController : Controller
    {
        #region Constructor
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<RoleRegisterVM> _roleRegisterValidator;
        private readonly IValidator<RoleEditVM> _roleEditValidator;
        private readonly IValidator<RoleEdit_Active_DisableVM> _roleEdit_DisableValidator;

        public AccessController(
            IRoleRepository roleRepository,
            IMapper mapper,
            IValidator<RoleRegisterVM> roleRegisterValidator,
            IValidator<RoleEditVM> roleEditValidator,
            IValidator<RoleEdit_Active_DisableVM> roleEdit_DisableValidator)
        {
            _roleRepository = roleRepository;
            _roleEditValidator = roleEditValidator;
            _roleRegisterValidator = roleRegisterValidator;
            _roleEdit_DisableValidator = roleEdit_DisableValidator;
            _mapper = mapper;
        }
        #endregion

        #region Select list

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
        public IActionResult GetUserRoles()
        {
            var roles = _roleRepository.GetUserRoles();

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
            return View();
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
        #endregion

    }
}