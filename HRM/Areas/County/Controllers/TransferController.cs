using Application.Services.Interfaces;
using AutoMapper;
using Domain.DTOs.Portal.Transfer;
using Domain.DTOs.Security.User;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRM.Areas.County.Controllers
{
    [Area("County")]
    public class TransferController : Controller
    {
        #region Constructor
        private readonly ITransferService _transferService;
        private readonly ITransferRepository _transferRepository;
        private readonly IDepartmentTransferRepository _departmentTransferRepository;
        private readonly IGeneralService _generalService;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IValidator<TransferRegisterVM> _transferRegisterValidator;
        private readonly IMapper _mapper;

        public TransferController(ITransferService transferService,
                                  IValidator<TransferRegisterVM> transferRegisterValidator,
                                  IGeneralService generalService,
                                  IUserRoleRepository userRoleRepository,
                                  ITransferRepository transferRepository,
                                  IDepartmentTransferRepository departmentTransferRepository,
                                  IMapper mapper)
        {
            _transferService = transferService;
            _transferRegisterValidator = transferRegisterValidator;
            _transferRepository = transferRepository;
            _generalService = generalService;
            _departmentTransferRepository = departmentTransferRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }
        #endregion

        #region Index
        public IActionResult CountyTransfersIndex()
        {
            return View();
        }
        #endregion

        #region Display
        public IActionResult FillTransfersGrid()
        {
            return View();
        }
        #endregion

        #region Register
        public IActionResult Register()
        {
            var proviveDepartments = _generalService.ProvinceDepartmentTypes();
            ViewBag.ProvinceDepartments = new SelectList(proviveDepartments, "Value", "Text");

            var countyDepartments = _generalService.CountyDepartmentTypes();
            ViewBag.CountyDepartments = new SelectList(countyDepartments, "Value", "Text");

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

                    model.IsActived = true;

                    bool result = _transferService.Register(model, out checkMessage);

                    if (result)
                    {
                        _transferRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات ارسال <span class='text-primary'> شهرستان </span> با موفقیت انجام شد.</h5>";
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
