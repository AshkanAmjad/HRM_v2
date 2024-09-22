using Application.Services.Interfaces;
using AutoMapper;
using Domain.DTOs.General;
using Domain.DTOs.Security.User;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]
    public class ArchiveController : Controller
    {
        #region Constructor
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IDocumentService _documentService;
        private readonly IDocumentRepository _documentRepository;
        private readonly IValidator<UserDelete_ActiveVM> _userDelete_ActiveValidator;
        private readonly IMapper _mapper;


        public ArchiveController(
            IUserService userService,
            IDocumentRepository documentRepository,
            IDocumentService documentService,
            IUserRepository userRepository,
            IValidator<UserDelete_ActiveVM> userDelete_ActiveValidator,
            IMapper mapper)
        {
            _userService = userService;
            _documentRepository = documentRepository;
            _userRepository = userRepository;
            _documentService = documentService;
            _userDelete_ActiveValidator = userDelete_ActiveValidator;
            _mapper = mapper;
        }
        #endregion

        #region Index
        public IActionResult ProvinceArchiveIndex()
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
            var users = _userRepository.GetArchivedUsers(arae);

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

        #region Delete

        [HttpPost]
        public IActionResult Delete(UserDelete_ActiveVM model)
        {
            ValidationResult userValidator = _userDelete_ActiveValidator.Validate(model);
            bool success = false;
            string message = $".عملیات حذف با شکست مواجه شده است";
            string checkMessage = "";
            if (userValidator.IsValid)
            {
                try
                {
                    bool result = _userRepository.Delete(model, out checkMessage);

                    if (result)
                    {
                        _userRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات حذف کاربر <span class='text-primary'> {model.UserName} </span> با موفقیت انجام شد.</h5>";
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

        #region Active
        [HttpPost]
        public IActionResult Active(UserDelete_ActiveVM model)
        {
            ValidationResult userValidator = _userDelete_ActiveValidator.Validate(model);
            bool success = false;
            string message = $".عملیات بازیابی با شکست مواجه شده است";
            string checkMessage = "";

            if (userValidator.IsValid)
            {
                try
                {
                    bool result = _userService.Active(model, out checkMessage);

                    if (result)
                    {
                        _userRepository.SaveChanges();

                        bool IsExistAvatarOnDb = _documentRepository.IsExistAvatarOnDb(model.UserId);

                        if (IsExistAvatarOnDb)
                        {
                            DirectionVM direction = _mapper.Map<DirectionVM>(model);

                            bool isExistOrginalAvatar = _documentService.IsExistOrginalAvatarOnServer(direction, model.UserName);
                            bool isExistThumbAvatar = _documentService.IsExistThumbAvatarOnServer(direction, model.UserName);

                            if (!isExistOrginalAvatar || !isExistThumbAvatar)
                            {
                                var avatar = _documentRepository.GetAvatarWithUserId(model.UserId);

                                if (!isExistOrginalAvatar)
                                    _documentRepository.DownloadOrginalAvatar(avatar);

                                if (!isExistThumbAvatar)
                                    _documentService.UploadDocumentToServer(avatar);
                            }
                        }

                        success = true;
                        message = $"<h5>عملیات بازیابی کاربر <span class='text-primary'> {model.UserName} </span> با موفقیت انجام شد.</h5>";
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

    }
}
