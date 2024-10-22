using Domain.DTOs.General;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]
    public class DocumentController : Controller
    {
        #region Constructor
        private readonly IDocumentRepository _documentRepository;

        public DocumentController(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }
        #endregion

        #region Index
        public IActionResult ProvinceManagementDocumentsIndex()
        {
            return View();
        }
        #endregion

        #region Display
        public IActionResult FillDocumentsGrid()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetDocuments(AreaVM model)
        {
            var documents = _documentRepository.GetDocuments(model);

            #region paging and searching
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";


            var mainData = documents
                .Where(d => d.UserName.Contains(searchValue) ||
                            d.Title.Contains(searchValue))
                .Skip(start)
                .Take(length)
                .ToList();

            var totalCount = documents
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

        #region Download
        public IActionResult Download(Guid documentId)
        {
            return NoContent();
        }
    }
}
