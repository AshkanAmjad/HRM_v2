﻿using Application.Services.Interfaces;
using AutoMapper;
using Data.Extensions;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Portal.Transfer;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRM.Areas.Province.Controllers
{
    [Area("Province")]
    [Authorize]

    public class TransferController : Controller
    {
        #region Constructor
        private readonly ITransferService _transferService;
        private readonly IUserRepository _userRepository;
        private readonly ITransferRepository _transferRepository;
        private readonly IDepartmentTransferRepository _departmentTransferRepository;
        private readonly IGeneralService _generalService;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IValidator<TransferRegisterVM> _transferRegisterValidator;
        private readonly IValidator<DownloadVM> _downloadValidator;
        private readonly IValidator<TransferActive_Disable_DescriptionVM> _transferActive_Disable_DescriptionValidator;
        private readonly IMapper _mapper;

        public TransferController(ITransferService transferService,
                                  IUserRepository userRepository,
                                  IValidator<TransferRegisterVM> transferRegisterValidator,
                                  IGeneralService generalService,
                                  IUserRoleRepository userRoleRepository,
                                  ITransferRepository transferRepository,
                                  IDepartmentTransferRepository departmentTransferRepository,
                                  IValidator<DownloadVM> downloadValidator,
                                  IValidator<TransferActive_Disable_DescriptionVM> transferActive_Disable_DescriptionValidator,
                                  IMapper mapper)
        {
            _transferService = transferService;
            _transferRegisterValidator = transferRegisterValidator;
            _transferRepository = transferRepository;
            _generalService = generalService;
            _departmentTransferRepository = departmentTransferRepository;
            _userRoleRepository = userRoleRepository;
            _downloadValidator = downloadValidator;
            _userRepository = userRepository;
            _transferActive_Disable_DescriptionValidator = transferActive_Disable_DescriptionValidator;
            _mapper = mapper;
        }
        #endregion

        #region Index
        [RolePermissionChecker("مدیریت", "فناوری اطلاعات")]
        public IActionResult ProvinceSendTransfersIndex()
        {
            return View();
        }

        [RolePermissionChecker("مدیریت", "فناوری اطلاعات")]
        public IActionResult ProvinceInboxTransfersIndex()
        {
            return View();
        }
        public IActionResult ManagementMyInboxTransfersIndex()
        {
            return View();
        }

        public IActionResult ManagementMySendTransfersIndex()
        {
            return View();
        }
        #endregion

        #region Display
        public IActionResult FillTransfersGrid()
        {
            return View();
        }

        [HttpPost]
        [RolePermissionChecker("مدیریت", "فناوری اطلاعات")]
        public IActionResult GetSendTransfers()
        {
            var id = User.Claims.Where(c => c.Type == "userId").FirstOrDefault().Value;

            if (id == "")
            {
                return NotFound();
            }

            var userId = new Guid(id);

            var userArea = _userRepository.GetAreaUserByUserId(userId);

            var transferArea = new TransferAreaVM();

            transferArea = new()
            {
                ReceiverArea = "0",
                UploaderArea = userArea.Section,
                UploaderCounty = userArea.County,
                UploaderDistrict = userArea.District,
                UploaderProvince = userArea.Province,
            };




            var transfers = _departmentTransferRepository.GetSendTransfers(transferArea);

            #region paging and searching
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";


            var filteredData = transfers.Where(t => t.Title.Contains(searchValue))
                                        .ToList();

            var mainData = filteredData.Skip(start)
                                       .Take(length)
                                       .ToList();

            var totalCount = transfers.Count();

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
        [RolePermissionChecker("مدیریت", "فناوری اطلاعات")]
        public IActionResult GetInboxTransfers()
        {
            var id = User.Claims.Where(c => c.Type == "userId").FirstOrDefault().Value;

            if (id == "")
            {
                return NotFound();
            }

            var userId = new Guid(id);

            var userArea = _userRepository.GetAreaUserByUserId(userId);

            var transferArea = new TransferAreaVM();


            transferArea = new TransferAreaVM()
            {
                UploaderArea = "0",
                ReceiverArea = userArea.Section,
                ReceiverCounty = userArea.County,
                ReceiverDistrict = userArea.District,
                ReceiverProvince = userArea.Province,
            };

            var transfers = _departmentTransferRepository.GetInboxTransfers(transferArea);

            #region paging and searching
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";


            var filteredData = transfers.Where(t => t.Title.Contains(searchValue))
                                        .ToList();

            var mainData = filteredData.Skip(start)
                                       .Take(length)
                                       .ToList();

            var totalCount = transfers.Count();

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
        [AreaPermissionChecker("0")]
        public IActionResult GetMySendTransfers()
        {
            var id = User.Claims.Where(c => c.Type == "userId").FirstOrDefault().Value;

            if (id == "")
            {
                return NotFound();
            }

            var userId = new Guid(id);

            var transferArea = new TransferAreaVM()
            {
                UploaderUserId = userId,
            };

            var transfers = _departmentTransferRepository.GetMySendTransfers(transferArea);

            #region paging and searching
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";


            var filteredData = transfers.Where(t => t.Title.Contains(searchValue))
                                        .ToList();

            var mainData = filteredData.Skip(start)
                                       .Take(length)
                                       .ToList();

            var totalCount = transfers.Count();

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
        [AreaPermissionChecker("0")]
        public IActionResult GetMyInboxTransfers()
        {
            var id = User.Claims.Where(c => c.Type == "userId").FirstOrDefault().Value;

            if (id == "")
            {
                return NotFound();
            }

            var userId = new Guid(id);

            var transferArea = new TransferAreaVM()
            {
                ReceiverUserId = userId,
                UploaderUserId = userId,
            };

            var transfers = _departmentTransferRepository.GetMyInboxTransfers(transferArea);

            #region paging and searching
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";


            var filteredData = transfers.Where(t => t.Title.Contains(searchValue))
                                        .ToList();

            var mainData = filteredData.Skip(start)
                                       .Take(length)
                                       .ToList();

            var totalCount = transfers.Count();

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

        #region Register
        public IActionResult Register()
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == "userId").Value;

            if (id == "")
            {
                return NotFound();
            }

            var userId = new Guid(id);

            var area = _userRepository.GetAreaUserByUserId(userId);

            var departments = _generalService.ProvinceDepartmentTypes();

            ViewBag.Departments = new SelectList(departments, "Value", "Text");

            var roles = _userRoleRepository.GetRolesForSelectBox();

            ViewBag.Roles = new SelectList(roles, "Value", "Text");

            if (area.Section != "0")
                return View("OtherRegister");

            return View();
        }

        [AreaPermissionChecker("0")]
        public IActionResult UserRegister()
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == "userId").Value;

            if (id == "")
            {
                return NotFound();
            }

            var userId = new Guid(id);

            var area = _userRepository.GetAreaUserByUserId(userId);

            var provinceDepartment = area.Province;
            var countyDepartment = area.County;
            var districtDepartment = area.District;

            ViewBag.ProvinceDepartment = provinceDepartment;
            ViewBag.CountyDepartment = countyDepartment;
            ViewBag.DistrictDepartment = districtDepartment;
            var roles = _userRoleRepository.GetRolesForSelectBox();

            ViewBag.Roles = new SelectList(roles, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(TransferRegisterVM model)
        {
            ValidationResult transferValidator = _transferRegisterValidator.Validate(model);
            bool success = false;
            var message = $"عملیات ارسال با شکست مواجه شده است.";
            string checkMessage = "";
            if (transferValidator.IsValid && User.Identity.IsAuthenticated)
            {
                try
                {
                    var id = User.Claims.Where(c => c.Type == "userId").FirstOrDefault().Value;

                    if (id == "")
                    {
                        return NotFound();
                    }

                    var userId = new Guid(id);

                    model.UserIdUploader = userId;

                    var userArea = _userRepository.GetAreaUserByUserId(userId);

                    if (userArea.Section == "2")
                    {
                        model.Display = false;
                    }
                    else
                    {
                        model.Display = true;
                    }

                    model.IsActived = true;


                    bool result = _transferService.Register(model, out checkMessage);

                    if (result)
                    {
                        _transferRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات ارسال <span class='text-primary'> استان </span> با موفقیت انجام شد.</h5>";
                    }
                    else
                    {
                        message = checkMessage;
                    }

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
                message = $"{transferValidator}";
            }
            #region Manual Validation
            foreach (var error in transferValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            transferValidator.AddToModelState(this.ModelState);
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

        #region Download
        [HttpGet]
        public IActionResult Download(DownloadVM model)
        {
            ValidationResult transferValidator = _downloadValidator.Validate(model);
            bool success = false;
            var message = $"عملیات بارگیری با شکست مواجه شده است.";
            if (transferValidator.IsValid)
            {
                try
                {
                    if (_transferRepository.IsExistTransferOnDb(model.DocumentId))
                    {
                        var item = _transferRepository.GetTransferById(model.DocumentId);
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
                message = $"{transferValidator}";
            }

            #region Manual Validation
            foreach (var error in transferValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            transferValidator.AddToModelState(this.ModelState);
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

        #region Disable

        [HttpPost]
        [RolePermissionChecker("مدیریت", "فناوری اطلاعات")]

        public IActionResult Disable(TransferActive_Disable_DescriptionVM transfer)
        {
            ValidationResult transferValidator = _transferActive_Disable_DescriptionValidator.Validate(transfer);
            bool success = false;
            var message = $"عملیات غیر فعال سازی با شکست مواجه شده است.";
            string checkMessage = "";
            if (transferValidator.IsValid)
            {
                try
                {
                    bool result = _transferRepository.Disable(transfer, out checkMessage);

                    if (result)
                    {
                        _transferRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات غیر فعال سازی تبادل با موفقیت انجام شد.</h5>";
                    }
                    else
                    {
                        message = checkMessage;
                    }
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
                message = $"{transferValidator}";
            }
            #region Manual Validation
            foreach (var error in transferValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            transferValidator.AddToModelState(this.ModelState);
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

        #region Active

        [HttpPost]
        [RolePermissionChecker("مدیریت", "فناوری اطلاعات")]

        public IActionResult Active(TransferActive_Disable_DescriptionVM transfer)
        {
            ValidationResult transferValidator = _transferActive_Disable_DescriptionValidator.Validate(transfer);
            bool success = false;
            var message = $"عملیات فعال سازی با شکست مواجه شده است.";
            string checkMessage = "";
            if (transferValidator.IsValid)
            {
                try
                {
                    bool result = _transferRepository.Active(transfer, out checkMessage);

                    if (result)
                    {
                        _transferRepository.SaveChanges();
                        success = true;
                        message = $"<h5>عملیات فعال سازی تبادل با موفقیت انجام شد.</h5>";
                    }
                    else
                    {
                        message = checkMessage;
                    }
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
                message = $"{transferValidator}";
            }
            #region Manual Validation
            foreach (var error in transferValidator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            transferValidator.AddToModelState(this.ModelState);
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

        #region Description
        public IActionResult GetDescription(TransferActive_Disable_DescriptionVM transfer)
        {
            ValidationResult transferValidator = _transferActive_Disable_DescriptionValidator.Validate(transfer);
            GetDescriptionVM result = new();
            if (transferValidator.IsValid)
            {

                result = _transferRepository.GetDescription(transfer);

            }

            return View(result);

        }
        #endregion

    }
}
