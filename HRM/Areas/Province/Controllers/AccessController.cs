using Application.Services.Implrmentations;
using Application.Services.Interfaces;
using AutoMapper;
using Data.Repositores;
using Domain.DTOs.General;
using Domain.DTOs.Security.Role;
using Domain.DTOs.Security.User;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using HRM.Models.Validation;
using HRM.Models.Validation.Security.Role;
using HRM.Models.Validation.Security.User;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]
    public class AccessController : Controller
    {
        #region Constructor
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<AssistantRegisterVM> _assistantRegisterValidator;
        private readonly IValidator<AssistantEditVM> _assistantEditValidator;
        private readonly IValidator<AssistantEdit_Active_DisableVM> _assistantEdit_DisableValidator;


        public AccessController(
            IRoleRepository roleRepository,
            IMapper mapper,
            IValidator<AssistantRegisterVM> assistantRegisterValidator,
            IValidator<AssistantEditVM> assistantEditValidator,
            IValidator<AssistantEdit_Active_DisableVM> assistantEdit_DisableValidator)
        {
            _roleRepository = roleRepository;
            _assistantEditValidator = assistantEditValidator;
            _assistantRegisterValidator = assistantRegisterValidator;
            _assistantEdit_DisableValidator = assistantEdit_DisableValidator;
            _mapper = mapper;
        }
        #endregion
        #region Index
        public IActionResult ProvinceAssistantsIndex()
        {
            return View();
        }

        public IActionResult ProvinceRolesIndex()
        {
            return View();
        }

        #endregion

        #region Display
        public IActionResult FillAssistantsGrid()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAssistants()
        {
            var assistants = _roleRepository.GetAssistants();

            #region paging and searching
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";


            var mainData = assistants
                .Where(a => a.Title.Contains(searchValue))
                .Skip(start)
                .Take(length)
                .ToList();

            var totalCount = assistants
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
        public IActionResult AssistantRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AssistantRegister(AssistantRegisterVM model)
        {
            ValidationResult assistantValidator = _assistantRegisterValidator.Validate(model);
            bool success = false;
            var message = $"عملیات ثبت با شکست مواجه شده است.";
            string checkMessage = "";
            if (assistantValidator.IsValid)
            {
                try
                {
                    bool result = _roleRepository.RegisterAssistant(model, out checkMessage);

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
                message = $"{assistantValidator}";
            }
            #region Manual Validation
            foreach (var error in assistantValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            assistantValidator.AddToModelState(this.ModelState);
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
        public IActionResult Edit(AssistantEdit_Active_DisableVM model)
        {
            ValidationResult assistantValidator = _assistantEdit_DisableValidator.Validate(model);

            if (assistantValidator.IsValid)
            {
                var assistant = _roleRepository.GetAssistantById(model.RoleId);

                if (assistant == null)
                    return NotFound();

                return View(assistant);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AssistantEditVM model)
        {
            ValidationResult assistantValidator = _assistantEditValidator.Validate(model);
            bool success = false;
            var message = $"عملیات ویرایش با شکست مواجه شده است.";
            string checkMessage = "";
            if (assistantValidator.IsValid)
            {
                try
                {
                    bool result = _roleRepository.EditAssistant(model, out checkMessage);

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
                message = $"{assistantValidator}";
            }
            #region Manual Validation
            foreach (var error in assistantValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            assistantValidator.AddToModelState(this.ModelState);
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
        public IActionResult Disable(AssistantEdit_Active_DisableVM model)
        {
            ValidationResult assistantValidator = _assistantEdit_DisableValidator.Validate(model);
            bool success = false;
            var message = $"عملیات غیر فعال سازی با شکست مواجه شده است.";
            string checkMessage = "";
            if (assistantValidator.IsValid)
            {
                try
                {
                    bool result = _roleRepository.DisableAssistant(model, out checkMessage);

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
                message = $"{assistantValidator}";
            }
            #region Manual Validation
            foreach (var error in assistantValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            assistantValidator.AddToModelState(this.ModelState);
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
