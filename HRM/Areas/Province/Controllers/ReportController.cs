﻿using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]
    public class ReportController : Controller
    {
        #region Constructor
        public ReportController() { }
        #endregion

        #region Index
        public ActionResult ProvinceReportsIndex()
        {
            return View();
        }
        #endregion

        #region Display

        #endregion
    }
}
