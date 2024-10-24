using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Areas.District.Controllers
{
    [Area("District")]

    public class DocumentController : Controller
    {
        #region Constructor
        private readonly IDocumentRepository _documentRepository;
        private readonly IValidator<DownloadVM> _downloadValidator;

        public DocumentController(IDocumentRepository documentRepository,
                                  IValidator<DownloadVM> downloadValidator)
        {
            _documentRepository = documentRepository;
            _downloadValidator = downloadValidator;
        }
        #endregion

        #region Index
        public IActionResult DistrictManagementDocumentsIndex()
        {
            return View();
        }
        public IActionResult DistrictArchiveDocumentsIndex()
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
        public IActionResult GetDocuments(AreaVM model, bool status)
        {
            var documents = _documentRepository.GetDocuments(model, status);

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
        [HttpGet]
        public IActionResult Download(DownloadVM model)
        {
            ValidationResult documentValidator = _downloadValidator.Validate(model);
            bool success = false;
            var message = $"عملیات بارگیری با شکست مواجه شده است.";
            if (documentValidator.IsValid)
            {
                try
                {
                    if (_documentRepository.IsExistDocumentOnDb(model.DocumentId))
                    {
                        var item = _documentRepository.GetDocumentById(model.DocumentId);

                        return File(item.Bytes, item.ContentType, item.FileName);
                    }

                    message = $"فایلی یافت نشد.";
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    message = $"خطای شکست عملیات  :  {ex.Message}";
                }

            }
            else
            {
                message = $"{documentValidator}";
            }

            #region Manual Validation
            foreach (var error in documentValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            documentValidator.AddToModelState(this.ModelState);
            #endregion

            #region Json data
            var jsonData = new
            {
                success = success,
                message = message,
            };
            #endregion

            return Json(jsonData);

        }

        #endregion
    }
}
