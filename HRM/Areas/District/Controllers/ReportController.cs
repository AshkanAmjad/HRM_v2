using Application.Extensions;
using Application.Services.Interfaces;
using AutoMapper;
using Data.Extensions;
using Domain.DTOs.General;
using Domain.DTOs.Security.Report;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRM.Areas.District.Controllers
{
    [Area("District")]
    [Authorize]
    public class ReportController : Controller
    {
        #region Constructor
        private readonly IReportRepository _reportRepository;
        private readonly IGeneralService _generalService;
        private readonly IMapper _mapper;


        public ReportController(IReportRepository reportRepository,
                                IGeneralService generalService,
                                IMapper mapper)

        {
            _mapper = mapper;
            _reportRepository = reportRepository;
            _generalService = generalService;
        }
        #endregion

        #region Index
        public ActionResult DistrictReportsIndex()
        {
            var provinceDepartments = _generalService.ProvinceDepartmentTypes();
            ViewBag.ProvinceDepartments = new SelectList(provinceDepartments, "Value", "Text");

            var countyDepartments = _generalService.CountyDepartmentTypes();
            ViewBag.CountyDepartments = new SelectList(countyDepartments, "Value", "Text");

            var districtDepartments = _generalService.DistrictDepartmentTypes();
            ViewBag.DistrictDepartments = new SelectList(districtDepartments, "Value", "Text");

            return View();
        }
        #endregion

        #region Display
        public IActionResult CountUsers(DisplayReportVM model)
        {
            var area = _mapper.Map<AreaVM>(model);
            area.Display = "2";

            var count = _reportRepository.GetCountUsers(area);

            if (count == null)
                return NotFound();

            return View(count);
        }
        public IActionResult PieChartOfRoles(DisplayReportVM model)
        {
            var data = _reportRepository.GetCountRoles(model);

            if (data == null)
                return NotFound();

            var colors = ColorGenerator.GenerateMultipleRandomColorHex(data.Count());

            ViewBag.PieColors = colors;

            return View(data);
        }

        public IActionResult DoughnutChartOfGenders(DisplayReportVM model)
        {
            var area = _mapper.Map<AreaVM>(model);
            
            var count = _reportRepository.GetCountGenders(area);

            if (count == null)
                return NotFound();

            return View(count);
        }

        public IActionResult BarChartOfEmployments(DisplayReportVM model)
        {
            var data = _reportRepository.GetCountEmoloyments(model);

            if (data == null)
                return NotFound();

            var colors = ColorGenerator.GenerateMultipleRandomColorHex(data.Count());

            ViewBag.BarColors = colors;

            return View(data);
        }

        public IActionResult BarChartOfEducations(DisplayReportVM model)
        {
            var data = _reportRepository.GetCountEducations(model);

            if (data == null)
                return NotFound();

            var colors = ColorGenerator.GenerateMultipleRandomColorHex(data.Count());

            ViewBag.BarColors = colors;

            return View(data);
        }

        public IActionResult MinHistoriesUsers(DisplayReportVM model)
        {
            var history = _reportRepository.CalculateMinHistory(model);

            if (history == null)
                return NotFound();

            return View(history);
        }

        public IActionResult MaxHistoriesUsers(DisplayReportVM model)
        {
            var history = _reportRepository.CalculateMaxHistory(model);

            if (history == null)
                return NotFound();

            return View(history);
        }

        public IActionResult BarChartOfHistories(DisplayReportVM model)
        {
            var hourlyWork = _reportRepository.CalculateHourlyWork(model);

            if (hourlyWork == null)
                return NotFound();

            return View(hourlyWork);
        }
        #endregion
    }
}