using Application.Services.Interfaces;
using AutoMapper;
using Data.Extensions;
using Data.Repositores;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Transfer;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.County.Controllers
{
    [Area("County")]
    [AreaPermissionChecker("1")]

    public class HomeController : Controller
    {
        #region Constructor
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IDocumentService _documentService;
        private readonly IDepartmentTransferRepository _departmentTransferRepository;
        private readonly IMapper _mapper;

        public HomeController(IUserRoleRepository userRoleRepository,
                              IDocumentService documentService,
                              IDepartmentTransferRepository departmentTransferRepository,
                              IMapper mapper)
        {
            _userRoleRepository = userRoleRepository;
            _documentService = documentService;
            _departmentTransferRepository = departmentTransferRepository;
            _mapper = mapper;
        }
        #endregion

        #region Main
        public IActionResult Main()
        {
            return View();
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Display 
        public async Task<IActionResult> FillDetailsGrid()
        {
            var id = User.Claims.Where(c => c.Type == "userId").FirstOrDefault().Value;

            if (id == "")
            {
                return NotFound();
            }

            var userId = new Guid(id);

            var user =await _userRoleRepository.GetUserDetailsAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            DirectionVM direction = _mapper.Map<DirectionVM>(user);
            bool IsExistAvatarOnDb = _documentService.CheckingAvatar(userId, user.UserName, direction);
            ViewData["IsExistAvatar"] = IsExistAvatarOnDb;

            return View(user);

        }


        public async Task<IActionResult> FillMyLatestTransfersGrid()
        {
            var id = User.Claims.Where(c => c.Type == "userId").FirstOrDefault().Value;

            if (id == "")
            {
                return NotFound();
            }

            var userId = new Guid(id);

            var transferArea = new TransferAreaVM()
            {
                ReceiverUserId = userId,
            };

            var transfers = await _departmentTransferRepository.GetMyLatestTransfersTransfersAsync(transferArea);

            return View(transfers);

        }
        #endregion
    }
}
