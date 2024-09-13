using Application.Services.Interfaces;
using AutoMapper;
using Domain.DTOs.General;
using Domain.DTOs.Security.User;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRM.Areas.County.Controllers
{
    [Area("County")]

    public class ManagementController : Controller
    {
        #region Constructor
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IDocumentService _documentService;
        private readonly IDocumentRepository _documentRepository;
        private readonly IValidator<UserRegisterVM> _userRegisterValidator;
        private readonly IValidator<UserEditVM> _userEditValidator;
        private readonly IValidator<UserEdit_DisableVM> _userEdit_DeleteValidator;
        private readonly IMapper _mapper;


        public ManagementController(IUserService userService,
            IValidator<UserRegisterVM> userRegisterValidator,
            IValidator<UserEditVM> userEditValidators,
            IValidator<UserEdit_DisableVM> userEdit_DeleteValidator,
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
            _userEdit_DeleteValidator = userEdit_DeleteValidator;
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

        public List<SelectListItem> CountyDepartmentTypes()
        {
            List<SelectListItem> countyDepartmentTypes = new()
            {
                new SelectListItem
                {
                    Text = "شعبه 1",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "شعبه 2",
                    Value = "2"
                },
                new SelectListItem
                {
                    Text = "شعبه 3",
                    Value = "3"
                }
            };
            return countyDepartmentTypes;
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
        public IActionResult CountyIndex()
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
    }
}
