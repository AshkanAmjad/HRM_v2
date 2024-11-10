using AutoMapper;
using Data.Context;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Portal.Transfer;
using Domain.Entities.Portal.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Data.Repositores
{
    public class TransferRepository : ITransferRepository
    {
        #region Constructor
        private readonly HRMContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;
        public TransferRepository(HRMContext context,
                                  IUserRepository userRepository,
                                  IUserRoleRepository userRoleRepository,
                                  IMapper mapper)
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _context = context;
            _mapper = mapper;
        }
        #endregion

        public bool Register(TransferRegisterVM model, out string message)
        {
            string checkMessage = "اطلاعات ناقص ارسال شده است.";
            if (model != null)
            {
                UploadTransferToDb(model);

                UploadDepartmentTransferToDb(model);

                message = "";
                return true;
            }
            message = checkMessage;
            return false;
        }

        public void UploadTransferToDb(TransferRegisterVM model)
        {
            if (model != null)
            {
                Transfer transfer = _mapper.Map<Transfer>(model);
                if (model.Document != null)
                {
                    var docName = Path.GetFileName(model.Document.FileName);
                    var fileExtension = Path.GetExtension(docName);
                    transfer.FileName = string.Concat($"{model.Title}-{model.UserName}", fileExtension);
                    transfer.FileFormat = fileExtension;

                    using (var target = new MemoryStream())
                    {
                        model.Document.CopyTo(target);
                        transfer.DataBytes = target.ToArray();
                    }

                }
                _context.Add(transfer);
            }
        }

        public void UploadDepartmentTransferToDb(TransferRegisterVM model)
        {
            if (model != null)
            {
                AreaVM area = _mapper.Map<AreaVM>(model);

                var departmentTransfers = new List<DepartmentTransfer>();

                var uploaderDepartmentId = _userRepository.GetDepartmentIdByUserId(model.UserIdUploader);

                if (model.UserIdReceiver == "" &&
                    model.RoleReceiver == "")
                {
                    var departmentIdsReceiver = _userRepository.GetDepartmentIds(area);

                    foreach (var item in departmentIdsReceiver)
                    {
                        departmentTransfers.Add(new DepartmentTransfer
                        {
                            DepartmentTransferId = new Guid(),
                            ReceiverDepartmentId = item,
                            UploaderDepartmentId = uploaderDepartmentId,
                            IsActived = model.IsActived,
                            TransferId = model.TransferId

                        });
                    }
                }
                else if (model.UserIdReceiver != "" &&
                    model.RoleReceiver == "")
                {
                    var receiverUserId = new Guid(model.UserIdReceiver);

                    var receiverDepartmentId = _userRepository.GetDepartmentIdByUserId(receiverUserId);

                    departmentTransfers.Add(new DepartmentTransfer
                    {
                        DepartmentTransferId = new Guid(),
                        ReceiverDepartmentId = receiverDepartmentId,
                        UploaderDepartmentId = uploaderDepartmentId,
                        IsActived = model.IsActived,
                        TransferId = model.TransferId
                    });

                }
                else if (model.UserIdReceiver == "" &&
                         model.RoleReceiver != "")
                {
                    var roleReceiver = new Guid(model.RoleReceiver);
                    var receiverdepartmentIds = _userRepository.GetDepartmentIdsByRoleId(roleReceiver, area);

                    foreach (var item in receiverdepartmentIds)
                    {
                        departmentTransfers.Add(new DepartmentTransfer
                        {
                            DepartmentTransferId = new Guid(),
                            ReceiverDepartmentId = item,
                            UploaderDepartmentId = uploaderDepartmentId,
                            IsActived = model.IsActived,
                            TransferId = model.TransferId
                        });
                    }
                }

                _context.DepartmentTransfers.AddRange(departmentTransfers);
            }
        }

        public DownloadDocumentVM? GetTransferById(Guid transferId)
        {
            var transfer = _context.Transfers.IgnoreQueryFilters()
                                             .Where(t => t.TransferId == transferId)
                                             .Select(t => new DownloadDocumentVM
                                             {
                                                 Bytes = t.DataBytes,
                                                 ContentType = t.ContentType,
                                                 FileName = t.FileName
                                             }
                                             ).FirstOrDefault();
            return transfer;
        }

        public bool IsExistTransferOnDb(Guid transferId)
            => _context.Transfers.IgnoreQueryFilters()
                                 .Where(t => t.TransferId == transferId)
                                 .Any();


        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public bool Disable(TransferActive_Disable_DescriptionVM model, out string message)
        {
            string checkMessage = "عملیات غیر فعال سازی با شکست مواجه شد.";
            if (model != null)
            {
                DisableTransferDb(model);

                message = "";
                return true;
            }
            message = checkMessage;
            return false;
        }

        public bool Active(TransferActive_Disable_DescriptionVM model, out string message)
        {
            string checkMessage = "عملیات فعال سازی با شکست مواجه شد.";
            if (model != null)
            {
                ActiveTransferDb(model);

                message = "";
                return true;
            }
            message = checkMessage;
            return false;
        }

        public void ActiveTransferDb(TransferActive_Disable_DescriptionVM model)
        {
            if (model != null)
            {
                var transfer = _context.Transfers
                                       .IgnoreQueryFilters()
                                       .Where(t => t.TransferId == model.TransferId)
                                       .FirstOrDefault();

                if (transfer != null)
                {
                    transfer.IsActived = true;
                    _context.Transfers.Update(transfer);
                }
            }
        }

        public void DisableTransferDb(TransferActive_Disable_DescriptionVM model)
        {
            if (model != null)
            {
                var transfer = _context.Transfers
                                       .Find(model.TransferId);

                if (transfer != null)
                {
                    transfer.IsActived = false;
                    _context.Transfers.Update(transfer);
                }
            }
        }

        public GetDescriptionVM GetDescription(TransferActive_Disable_DescriptionVM model)
        {
            var description = _context.Transfers.Where(t => t.TransferId == model.TransferId)
                                                .Select(t => new GetDescriptionVM
                                                {
                                                    Description = t.Description
                                                })
                                                .FirstOrDefault();
            return description;
        }

    }
}
