using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.District.Controllers
{
    [Area("District")]
    public class AccessController : Controller
    {
        #region Constructor
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public AccessController(
            IRoleRepository roleRepository,
            IMapper mapper)

        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }
        #endregion

        #region Index
        public IActionResult DistrictRolesIndex()
        {
            return View();
        }

        public IActionResult DistrictUserRolesIndex()
        {
            return View();
        }

        #endregion

        #region Display
        public IActionResult FillRolesGrid()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetRoles()
        {
            var roles = _roleRepository.GetRoles();

            #region paging and searching
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";


            var mainData = roles
                .Where(a => a.Title.Contains(searchValue))
                .Skip(start)
                .Take(length)
                .ToList();

            var totalCount = roles
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
