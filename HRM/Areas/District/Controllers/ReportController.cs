using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.District.Controllers
{
    [Area("District")]

    public class ReportController : Controller
    {
        #region Constructor
        public ReportController() { }
        #endregion

        #region Index
        public ActionResult DistrictReportsIndex()
        {
            return View();
        }
        #endregion

        #region Display

        #endregion
    }
}
