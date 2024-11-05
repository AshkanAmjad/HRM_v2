using AutoMapper;
using Data.Context;
using Domain.DTOs.General;
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

                var departmentIdUploader = _userRepository.GetDepartmentIdByUserId(model.UserIdUploader);

                if (model.UserIdReceiver == "" &&
                    model.RoleReceiver == "")
                {
                    var departmentIdsReceiver = _userRepository.GetDepartmentIds(area);

                    foreach (var item in departmentIdsReceiver)
                    {
                        departmentTransfers.Add(new DepartmentTransfer
                        {
                            DepartmentTransferId = new Guid(),
                            DepartmentIdReceiver = item,
                            DepartmentIdUploader = departmentIdUploader,
                            IsActived = model.IsActived,
                            TransferId = model.TransferId

                        });
                    }
                }
                else if (model.UserIdReceiver != "" &&
                    model.RoleReceiver == "")
                {
                    var userIdReceiver = new Guid(model.UserIdReceiver);

                    var departmentIdReceiver = _userRepository.GetDepartmentIdByUserId(userIdReceiver);

                    departmentTransfers.Add(new DepartmentTransfer
                    {
                        DepartmentTransferId = new Guid(),
                        DepartmentIdReceiver = departmentIdReceiver,
                        DepartmentIdUploader = departmentIdUploader,
                        IsActived = model.IsActived,
                        TransferId = model.TransferId
                    });

                }
                else if (model.UserIdReceiver == "" &&
                         model.RoleReceiver != "")
                {
                    var roleReceiver = new Guid(model.RoleReceiver);
                    var departmentIdsReceveiver = _userRepository.GetDepartmentIdsByRoleId(roleReceiver, area);

                    foreach(var item in departmentIdsReceveiver)
                    {
                        departmentTransfers.Add(new DepartmentTransfer
                        {
                            DepartmentTransferId = new Guid(),
                            DepartmentIdReceiver = item,
                            DepartmentIdUploader = departmentIdUploader,
                            IsActived = model.IsActived,
                            TransferId = model.TransferId
                        });
                    }
                }

                _context.DepartmentTransfers.AddRange(departmentTransfers);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
