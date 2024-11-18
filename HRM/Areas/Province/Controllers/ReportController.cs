using Application.Extensions;
using Data.Extensions;
using Domain.DTOs.Security.Report;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]
    [Authorize]
    [RolePermissionChecker("مدیریت", "فناوری اطلاعات")]
    public class ReportController : Controller
    {
        #region Constructor
        private readonly IReportRepository _reportRepository;
        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        #endregion

        #region Index
        public ActionResult ProvinceReportsIndex()
        {
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
