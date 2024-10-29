using Application.Services.Implrmentations;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.DTOs.Portal.Transfer;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]

    public class TransferController : Controller
    {
        #region Constructor
        private readonly ITransferService _transferService;
        private readonly IGeneralService _generalService;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IValidator<TransferRegisterVM> _transferRegisterValidator;
        private readonly IMapper _mapper;

        public TransferController(ITransferService transferService,
                                  IValidator<TransferRegisterVM> transferRegisterValidator,
                                  IGeneralService generalService,
                                  IUserRoleRepository userRoleRepository,
                                  IMapper mapper)
        {
            _transferService = transferService;
            _transferRegisterValidator = transferRegisterValidator;
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
        #endregion
    }
}
