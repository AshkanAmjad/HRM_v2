using Data.Extensions;
using Data.Repositores;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]
    [Authorize]
    [AreaPermissionChecker("0")]

    public class DocumentController : Controller
    {
        #region Constructor
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<DownloadVM> _downloadValidator;

        public DocumentController(IDocumentRepository documentRepository,
                                  IValidator<DownloadVM> downloadValidator,
                                  IUserRepository userRepository)
        {
            _documentRepository = documentRepository;
            _downloadValidator = downloadValidator;
            _userRepository = userRepository;
        }
        #endregion

        #region Index
        [RolePermissionChecker("مدیریت", "فناوری اطلاعات")]
        public IActionResult ProvinceManagementDocumentsIndex()
        {
            return View();
        }

        [RolePermissionChecker("مدیریت", "فناوری اطلاعات")]
        public IActionResult ProvinceArchiveDocumentsIndex()
        {
            return View();
        }

        public IActionResult ManagementMyDocumentsIndex()
        {
            return View();
        }

        #endregion

        #region Display

        [RolePermissionChecker("مدیریت", "فناوری اطلاعات")]
        public IActionResult FillDocumentsGrid()
        {
            return View();
        }

        public IActionResult FillMyDocumentsGrid()
        {
            return View();
        }

        [HttpPost]
        [RolePermissionChecker("مدیریت", "فناوری اطلاعات")]

        public IActionResult GetDocuments(bool status)
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == "userId").Value;

            if (id == "")
            {
                return NotFound();
            }

            var userId = new Guid(id);

            var area = _userRepository.GetAreaUserByUserId(userId);
            area.Display = "0";

            var documents = _documentRepository.GetDocuments(area, status);

            #region paging and searching
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";

            var filteredData = documents.Where(d => d.UserName.Contains(searchValue) ||
                                                    d.Title.Contains(searchValue))
                                        .ToList();

            var mainData = filteredData.Skip(start)
                                       .Take(length)
                                       .ToList();

            var totalCount = documents.Count();

            var filteredCount = filteredData.Count();

            #endregion

            var jsonData = new
            {
                draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0"),
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = mainData
            };

            return Json(jsonData);
        }

        [HttpPost]
        public IActionResult GetMyDocuments()
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == "userId").Value;

            if (id == "")
            {
                return NotFound();
            }

            var userId = new Guid(id);

            var model = _userRepository.GetAreaUserByUserId(userId);

            var documents = _documentRepository.GetMyDocuments(model, userId);

            #region paging and searching
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";

            var filteredData = documents.Where(d => d.Title.Contains(searchValue))
                                        .ToList();

            var mainData = filteredData.Skip(start)
                                       .Take(length)
                                       .ToList();

            var totalCount = documents.Count();

            var filteredCount = filteredData.Count();

            #endregion

            var jsonData = new
            {
                draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0"),
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
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
