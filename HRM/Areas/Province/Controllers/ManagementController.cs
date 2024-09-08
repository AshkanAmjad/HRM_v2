using Application.Services.Interfaces;
using AutoMapper;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.User;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using System.Text.Json;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]

    public class ManagementController : Controller
    {
        #region Constructor
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IDocumentService _documentService;
        private readonly IDocumentRepository _documentRepository;
        private readonly IValidator<UserRegisterVM> _userRegisterValidator;
        private readonly IValidator<UserEditVM> _userEditValidator;
        private readonly IValidator<UserDeleteVM> _userDeleteValidator;
        private readonly IMapper _mapper;


        public ManagementController(IUserService userService,
            IValidator<UserRegisterVM> userRegisterValidator,
            IValidator<UserEditVM> userEditValidators,
            IValidator<UserDeleteVM> userDeleteValidator,
            IUserRepository userRepository,
            IDocumentService documentService,
            IDocumentRepository documentRepository,
            IMapper mapper)
        {
            _userService = userService;
            _userRegisterValidator = userRegisterValidator;
            _userRepository = userRepository;
            _documentService = documentService;
            _userEditValidator = userEditValidators;
            _documentRepository = documentRepository;
            _mapper = mapper;
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

        public List<SelectListItem> EducationTypes()
        {
            List<SelectListItem> educationTypes = new()
            {
                new SelectListItem
                {
                    Text="دیپلم",
                    Value="Dip"
                },
                new SelectListItem
                {
                    Text="کارشناسی",
                    Value="B"
                },
                new SelectListItem
                {
                    Text="کارشناسی ارشد",
                    Value="M"
                },
                new SelectListItem
                {
                    Text="دکترا",
                    Value="D"
                }
            };
            return educationTypes;
        }

        public List<SelectListItem> MariltalTypes()
        {
            List<SelectListItem> maritalTypes = new()
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
            return maritalTypes;
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

        #region Index
        public IActionResult ProvinceIndex()
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
        public IActionResult GetUsers(AreaVM arae)
        {
            var users = _userService.GetUsers(arae);

            #region paging and searching
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";


            var mainData = users
                .Where(u => u.UserName.Contains(searchValue))
                .Skip(start)
                .Take(length)
                .ToList();

            var totalCount = users
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
        public IActionResult Register()
        {
            var genders = GenderTypes();
            var marital = MariltalTypes();
            var employment = EmploymentTypes();
            var education = EducationTypes();
            ViewData["Gendes"] = genders;
            ViewData["Marital"] = marital;
            ViewData["Employment"] = employment;
            ViewData["Education"] = education;
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
                    message = $"<h5>خطای شکست عملیات  : {ex.Message} </h5>";
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
        public IActionResult Edit(Guid userId, int province, int county, int district)
        {
            AreaVM area = new()
            {
                County = county,
                District = district,
                Province = province
            };

            if (userId == Guid.Empty)
            {
                return NotFound();
            }

            var user = _userRepository.GetUserById(userId, area);

            if (user == null)
            {
                return NotFound();
            }

            bool IsExistAvatarOnDb = _documentRepository.IsExistAvatarOnDb(user.UserId);

            if (IsExistAvatarOnDb)
            {
                bool isExistOrginalAvatar = _documentService.IsExistOrginalAvatarOnServer(user);
                bool isExistThumbAvatar = _documentService.IsExistThumbAvatarOnServer(user);

                if (!isExistOrginalAvatar || !isExistThumbAvatar)
                {
                    var avatar = _documentRepository.GetAvatarWithUserId(user.UserId);

                    if (!isExistOrginalAvatar)
                        _documentRepository.DownloadOrginalAvatar(avatar);

                    if (!isExistThumbAvatar)
                        _documentService.UploadDocumentToServer(avatar);
                }
            }

            var genders = GenderTypes();
            var marital = MariltalTypes();
            var employment = EmploymentTypes();
            var education = EducationTypes();
            ViewData["Gendes"] = genders;
            ViewData["Marital"] = marital;
            ViewData["Employment"] = employment;
            ViewData["Education"] = education;
            ViewData["IsExistAvatar"] = IsExistAvatarOnDb;

            return View(user);
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
                    message = $"<h5>خطای شکست عملیات  : {ex.Message} </h5>";
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

        #region Delete

        [HttpPost]
        public IActionResult Delete(UserDeleteVM user)
        {
            ValidationResult userValidator = _userDeleteValidator.Validate(user);
            bool success = false;
            var message = $"عملیات غیر فعال سازی با شکست مواجه شده است.";
            string checkMessage = "";
            if (userValidator.IsValid)
            {
                try
                {
                    bool result = _userService.Delete(user, out checkMessage);

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
                    message = $"<h5>خطای شکست عملیات  : {ex.Message} </h5>";
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
        public IActionResult Profile()
        {
            return View();
        }
        #endregion
    }
}
