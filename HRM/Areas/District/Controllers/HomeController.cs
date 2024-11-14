using Application.Services.Interfaces;
using AutoMapper;
using Data.Extensions;
using Domain.DTOs.General;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.District.Controllers
{
    [Area("District")]
    [AreaPermissionChecker("2")]

    public class HomeController : Controller
    {
        #region Constructor
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;

        public HomeController(IUserRoleRepository userRoleRepository,
                              IDocumentService documentService,
                              IMapper mapper)
        {
            _userRoleRepository = userRoleRepository;
            _documentService = documentService;
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

        #region Display details
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
        #endregion

    }
}
