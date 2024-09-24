using Application.Services.Interfaces;
using AutoMapper;
using Domain.DTOs.General;
using Domain.DTOs.Security.User;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]
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
    }
}
