using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.County.Controllers
{
    [Area("County")]

    public class ReportController : Controller
    {
        #region Constructor
        public ReportController() { }
        #endregion

        #region Index
        public ActionResult CountyReportsIndex()
        {
            return View();
        }
        #endregion

        #region Display

        #endregion
    }
}
