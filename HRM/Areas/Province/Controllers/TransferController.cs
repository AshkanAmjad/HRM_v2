using Application.Services.Implrmentations;
using Application.Services.Interfaces;
using AutoMapper;
using Data.Repositores;
using Domain.DTOs.Portal.Transfer;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]

    public class TransferController : Controller
    {
        #region Constructor
        private readonly ITransferService _transferService;
        private readonly ITransferRepository _transferRepository;
        private readonly IGeneralService _generalService;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IValidator<TransferRegisterVM> _transferRegisterValidator;
        private readonly IMapper _mapper;

        public TransferController(ITransferService transferService,
                                  IValidator<TransferRegisterVM> transferRegisterValidator,
                                  IGeneralService generalService,
                                  IUserRoleRepository userRoleRepository,
                                  ITransferRepository transferRepository,
                                  IMapper mapper)
        {
            _transferService = transferService;
            _transferRegisterValidator = transferRegisterValidator;
            _transferRepository = transferRepository;
            _generalService = generalService;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }
        #endregion

        #region Index
        public IActionResult ProvinceTransfersIndex()
        {
            return View();
        }
        #endregion

        #region Register
        public IActionResult Register()
        {
            var departments = _generalService.ProvinceDepartmentTypes();
            ViewBag.Departments = new SelectList(departments, "Value", "Text");

            var roles = _userRoleRepository.GetRolesForSelectBox();

            ViewBag.Roles = new SelectList(roles, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(TransferRegisterVM model)
        {
            ValidationResult transferValidator = _transferRegisterValidator.Validate(model);
            bool success = false;
            var message = $"عملیات ارسال با شکست مواجه شده است.";
            string checkMessage = "";
            if (transferValidator.IsValid && User.Identity.IsAuthenticated)
            {
                try
                {
                    var id = User.Claims.FirstOrDefault(c => c.Type == "userId").Value;

                    if (id == "")
                    {
                        return NotFound();
                    }

                    var userId = new Guid(id);

                    model.UserIdUploader = userId;
                    
                    bool result = _transferService.Register(model, out checkMessage);

                    if (result)
                    {
                        _transferRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات ارسال <span class='text-primary'> استان - استان </span> با موفقیت انجام شد.</h5>";
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
                message = $"{transferValidator}";
            }
            #region Manual Validation
            foreach (var error in transferValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            transferValidator.AddToModelState(this.ModelState);
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
