using Application.Services.Interfaces;
using AutoMapper;
using Domain.DTOs.General;
using Domain.DTOs.Security.Profile;
using Domain.DTOs.Security.User;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]

    public class ManagementController : Controller
    {
        #region Constructor
        private readonly IUserService _userService;
        private readonly IGeneralService _generalService;
        private readonly IUserRepository _userRepository;
        private readonly IDocumentService _documentService;
        private readonly IDocumentRepository _documentRepository;
        private readonly IValidator<UserRegisterVM> _userRegisterValidator;
        private readonly IValidator<UserEditVM> _userEditValidator;
        private readonly IValidator<UserEdit_DisableVM> _userEdit_DeleteValidator;
        private readonly IValidator<ProfileEditVM> _profileEditValidator;
        private readonly IMapper _mapper;


        public ManagementController(IUserService userService,
            IValidator<UserRegisterVM> userRegisterValidator,
            IValidator<UserEditVM> userEditValidator,
            IValidator<UserEdit_DisableVM> userEdit_DeleteValidator,
            IValidator<ProfileEditVM> profileEditValidator,
            IUserRepository userRepository,
            IDocumentService documentService,
            IGeneralService generalService,
            IDocumentRepository documentRepository,
            IMapper mapper)
        {
            _userService = userService;
            _userRegisterValidator = userRegisterValidator;
            _userRepository = userRepository;
            _documentService = documentService;
            _profileEditValidator = profileEditValidator;
            _userEditValidator = userEditValidator;
            _userEdit_DeleteValidator = userEdit_DeleteValidator;
            _documentRepository = documentRepository;
            _generalService = generalService;
            _mapper = mapper;
        }
        #endregion

        #region Index
        public IActionResult ProvinceManagementUsersIndex()
        {
            return View();
        }
        #endregion

        #region Display
        public IActionResult FillUsersGrid()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetUsers(AreaVM model)
        {
            var users = _userRepository.GetUsers(model);

            #region paging and searching
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";

            var filteredData = users.Where(u => u.UserName.Contains(searchValue))
                                    .ToList();

            var mainData = filteredData.Skip(start)
                                       .Take(length)
                                       .ToList();

            var totalCount = users.Count();

            var filteredCount = filteredData.Count();

            #endregion


            var jsonData = new
            {
                draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0"),
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = mainData
            };

            return Json(jsonData);
        }

        public IActionResult FillProfileGrid()
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == "userId").Value;

            if (id == "")
            {
                return NotFound();
            }

            var userId = new Guid(id);

            var user = _userRepository.GetProfileById(userId);

            if (user == null)
            {
                return NotFound();
            }

            DirectionVM direction = _mapper.Map<DirectionVM>(user);

            bool IsExistAvatarOnDb = _documentService.CheckingAvatar(user.UserId, user.UserName, direction);

            ViewData["IsExistAvatar"] = IsExistAvatarOnDb;

            return View(user);
        }
        #endregion

        #region Register
        public IActionResult Register()
        {
            var genders = _generalService.GenderTypes();
            var marital = _generalService.MariltalTypes();
            var employment = _generalService.EmploymentTypes();
            var education = _generalService.EducationTypes();
            var departments = _generalService.ProvinceDepartmentTypes();
            ViewBag.Genders = new SelectList(genders, "Value", "Text");
            ViewBag.Marital = new SelectList(marital, "Value", "Text");
            ViewBag.Employment = new SelectList(employment, "Value", "Text");
            ViewBag.Education = new SelectList(education, "Value", "Text"); ;
            ViewBag.Departments = new SelectList(departments, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserRegisterVM user)
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
                        _userRepository.SaveChanges();
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
                    message = $"خطای شکست عملیات  : {ex.Message}";
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
        public IActionResult Edit(UserEdit_DisableVM model)
        {
            ValidationResult userValidator = _userEdit_DeleteValidator.Validate(model);

            if (userValidator.IsValid)
            {
                var user = _userRepository.GetUserById(model.UserId);

                if (user == null)
                {
                    return NotFound();
                }

                DirectionVM direction = _mapper.Map<DirectionVM>(user);

                bool IsExistAvatarOnDb = _documentService.CheckingAvatar(user.UserId, user.UserName, direction);

                var genders = _generalService.GenderTypes();
                var marital = _generalService.MariltalTypes();
                var employment = _generalService.EmploymentTypes();
                var education = _generalService.EducationTypes();
                var departments = _generalService.ProvinceDepartmentTypes();
                ViewBag.Genders = new SelectList(genders, "Value", "Text");
                ViewBag.Marital = new SelectList(marital, "Value", "Text");
                ViewBag.Employment = new SelectList(employment, "Value", "Text");
                ViewBag.Education = new SelectList(education, "Value", "Text"); ;
                ViewBag.Departments = new SelectList(departments, "Value", "Text");

                ViewData["IsExistAvatar"] = IsExistAvatarOnDb;

                return View(user);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserEditVM user)
        {
            ValidationResult userValidator = _userEditValidator.Validate(user);
            bool success = false;
            var message = $"عملیات ویرایش با شکست مواجه شده است.";
            string checkMessage = "";
            if (userValidator.IsValid)
            {
                try
                {
                    bool result = _userService.Edit(user, out checkMessage);

                    if (result)
                    {
                        _userRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات ویرایش کاربر <span class='text-primary'> {user.FirstName}  {user.LastName} </span> با موفقیت انجام شد.</h5>";
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
        public IActionResult Disable(UserEdit_DisableVM user)
        {
            ValidationResult userValidator = _userEdit_DeleteValidator.Validate(user);
            bool success = false;
            var message = $"عملیات غیر فعال سازی با شکست مواجه شده است.";
            string checkMessage = "";
            if (userValidator.IsValid)
            {
                try
                {
                    bool result = _userService.Disable(user, out checkMessage);

                    if (result)
                    {
                        _userRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات غیر فعال سازی کاربر <span class='text-primary'> {user.UserName} </span> با موفقیت انجام شد.</h5>";
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
            var jsonData = new
            {
                success = success,
                message = message,
            };
            #endregion

            return Json(jsonData);
        }
        #endregion

        #region Profile
        public IActionResult EditProfile(UserEdit_DisableVM model)
        {
            ValidationResult userValidator = _userEdit_DeleteValidator.Validate(model);

            if (userValidator.IsValid)
            {
                var user = _userRepository.GetUserProfileById(model.UserId);

                if (user == null)
                {
                    return NotFound();
                }

                DirectionVM direction = _mapper.Map<DirectionVM>(user);

                bool IsExistAvatarOnDb = _documentService.CheckingAvatar(user.UserId, user.UserName, direction);

                var marital = _generalService.MariltalTypes();
                var education = _generalService.EducationTypes();
                ViewBag.Marital = new SelectList(marital, "Value", "Text");
                ViewBag.Education = new SelectList(education, "Value", "Text"); ;

                ViewData["IsExistAvatar"] = IsExistAvatarOnDb;
                return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(ProfileEditVM user)
        {
            ValidationResult userValidator = _profileEditValidator.Validate(user);
            bool success = false;
            var message = $"عملیات ویرایش با شکست مواجه شده است.";
            string checkMessage = "";
            if (userValidator.IsValid)
            {
                try
                {
                    bool result = _userService.Edit(user, out checkMessage);

                    if (result)
                    {
                        _userRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات ویرایش کاربر <span class='text-primary'> {user.FirstName}  {user.LastName} </span> با موفقیت انجام شد.</h5>";
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
