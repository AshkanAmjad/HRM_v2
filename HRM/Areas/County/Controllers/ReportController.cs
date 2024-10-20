using Application.Extensions;
using Application.Services.Interfaces;
using Domain.DTOs.Security.Report;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRM.Areas.County.Controllers
{
    [Area("County")]

    public class ReportController : Controller
    {
        #region Constructor
        private readonly IReportRepository _reportRepository;
        private readonly IGeneralService _generalService;

        public ReportController(IReportRepository reportRepository,
                                IGeneralService generalService)
        {
            _generalService = generalService;
            _reportRepository = reportRepository;
        }
        #endregion

        #region Index
        public ActionResult CountyReportsIndex()
        {
            var proviveDepartments = _generalService.ProvinceDepartmentTypes();
            ViewBag.ProvinceDepartments = new SelectList(proviveDepartments, "Value", "Text");

            var countyDepartments = _generalService.CountyDepartmentTypes();
            ViewBag.CountyDepartments = new SelectList(countyDepartments, "Value", "Text");
            return View();
        }
        #endregion

        #region Display
        public IActionResult CountUsers(DisplayReportVM model)
        {
            var count = _reportRepository.GetCountUsers(model);

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
            var count = _reportRepository.GetCountGenders(model);

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

            if(history == null)
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
