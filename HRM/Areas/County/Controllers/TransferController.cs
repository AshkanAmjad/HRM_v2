using Application.Services.Interfaces;
using AutoMapper;
using Domain.DTOs.Portal.Transfer;
using Domain.DTOs.Security.User;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.County.Controllers
{
    [Area("County")]

    public class TransferController : Controller
    {
        #region Constructor
        private readonly ITransferService _transferService;
        private readonly IValidator<TransferRegisterVM> _transferRegisterValidator;
        private readonly IMapper _mapper;

        public TransferController(ITransferService transferService,
                                  IValidator<TransferRegisterVM> transferRegisterValidator,
                                  IMapper mapper)
        {
            _transferService = transferService;
            _transferRegisterValidator = transferRegisterValidator;
            _mapper = mapper;
        }
        #endregion

        #region Index
        public IActionResult CountyTransfersIndex()
        {
            return View();
        }
        #endregion
    }
}
