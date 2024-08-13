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
        private readonly IMapper _mapper;


        public ManagementController(IUserService userService,
            IValidator<UserRegisterVM> userRegisterValidator,
            IUserRepository userRepository,
            IDocumentService documentService,
            IDocumentRepository documentRepository,
            IMapper mapper)
        {
            _userService = userService;
            _userRegisterValidator = userRegisterValidator;
            _userRepository = userRepository;
            _documentService = documentService;
            _documentRepository = documentRepository;
            _mapper=mapper;
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


            var mainData=users
                .Where(u => u.UserName.Contains(searchValue))
                .Skip(start)
                .Take(length)
                .ToList();

            var totalCount=users
                .Count();

            #endregion


            var jsonData = new
            {
                draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0"),
                recordTotal =totalCount,
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
        public IActionResult Edit(Guid userId,int province,int county,int district)
        {
            AreaVM area = new()
            {
                County = county,
                District = district,
                Province = province
            };
            
            if(userId == Guid.Empty)
            {
                return NotFound();
            }

            var user=_userRepository.GetUserById(userId, area);
            
            if(user == null)
            {
                return NotFound();
            }

            if (!_documentService.IsExistAvatarOnServer(user)
                &&
                _documentRepository.IsExistAvatarOnDb(user.UserId))
            {
                var avatar=_documentRepository.GetAvatarWithUserId(user.UserId);
                UploadVM document= _mapper.Map<UploadVM>(avatar);
                _documentRepository.DownloadOrginalAvatar(avatar);
                _documentService.UploadDocumentToServer(document);
            }

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
        public IActionResult Edit()
        {
            return View();
        }
        #endregion

        #region Delete
        public IActionResult Delete(Guid userId, int province, int county, int district)
        {
            AreaVM area = new()
            {
                County = county,
                District = district,
                Province = province
            };

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete()
        {
            return View();
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
