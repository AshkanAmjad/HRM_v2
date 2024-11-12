using AutoMapper;
using Data.Context;
using Data.Extensions;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Transfer;
using Domain.Entities.Portal.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositores
{
    public class DepartmentTransferRepository : IDepartmentTransferRepository
    {
        #region Constructor
        private readonly HRMContext _context;
        private readonly IMapper _mapper;

        public DepartmentTransferRepository(HRMContext context,
                                            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        public IQueryable<DepartmentTransfer> GetSendTransfersQuery(TransferAreaVM area)
          => _context.DepartmentTransfers.IgnoreQueryFilters()
                                         .Where(dt => dt.UploaderDepartment.Area == area.UploaderArea &&
                                                     dt.UploaderDepartment.Province == area.UploaderProvince &&
                                                     dt.UploaderDepartment.County == area.UploaderCounty &&
                                                     dt.UploaderDepartment.District == area.UploaderDistrict &&
                                                     dt.ReceiverDepartment.Area == area.ReceiverArea
                                                     )
                                         .AsQueryable();

        public List<DisplayTransfersVM> GetSendTransfers(TransferAreaVM area)
        {
            var context = GetSendTransfersQuery(area);
            var transfers = (from item in context
                             orderby item.Transfer.UploadDate descending
                             select new DisplayTransfersVM
                             {
                                 TransferId = item.Transfer.TransferId,
                                 Title = item.Transfer.Title,
                                 NationalCodeUploader = item.UploaderDepartment.User.UserName,
                                 RoleUploader = string.Join("\n", item.UploaderDepartment.User.UserRoles.Select(ur => ur.Role.Title)),
                                 AreaUploader = item.UploaderDepartment.Area,
                                 ProvinceUploader = item.UploaderDepartment.Province,
                                 CountyUploader = item.UploaderDepartment.County,
                                 DistrictUploader = item.UploaderDepartment.District,
                                 NationalCodeReceiver = item.ReceiverDepartment.User.UserName,
                                 RoleReceiver = item.ReceiverDepartment.User.UserRoles.Count != 0
                                               ? string.Join("\n", item.ReceiverDepartment.User.UserRoles.Select(ur => ur.Role.Title))
                                               : "-",
                                 AreaReceiver = item.ReceiverDepartment.Area,
                                 ProvinceReceiver = item.ReceiverDepartment.Province,
                                 CountyReceiver = item.ReceiverDepartment.County,
                                 DistrictReceiver = item.ReceiverDepartment.District,
                                 FileFormat = (item.Transfer.FileFormat != null ? item.Transfer.FileFormat : "-"),
                                 UploadDate = item.Transfer.UploadDate.ToShamsi(),
                                 IsActived = (item.Transfer.IsActived) ? "فعال" : "غیرفعال",
                             }).ToList();
            return transfers;
        }

        public IQueryable<DepartmentTransfer> GetInboxTransfersQuery(TransferAreaVM area)
          => _context.DepartmentTransfers.IgnoreQueryFilters()
                                         .Where(dt => dt.ReceiverDepartment.Area == area.ReceiverArea &&
                                                      dt.UploaderDepartment.Area == area.UploaderArea &&
                                                      dt.ReceiverDepartment.Province == area.ReceiverProvince &&
                                                      dt.ReceiverDepartment.County == area.ReceiverCounty &&
                                                      dt.ReceiverDepartment.District == area.ReceiverDistrict &&
                                                      dt.IsActived
                                         ) 
                                         .AsQueryable();
        

        public List<DisplayTransfersVM> GetInboxTransfers(TransferAreaVM area)
        {
            var context = GetInboxTransfersQuery(area);
            var transfers = (from item in context
                             orderby item.Transfer.UploadDate descending
                             select new DisplayTransfersVM
                             {
                                 TransferId = item.Transfer.TransferId,
                                 Title = item.Transfer.Title,
                                 NationalCodeUploader = item.UploaderDepartment.User.UserName,
                                 RoleUploader = string.Join("\n", item.UploaderDepartment.User.UserRoles.Select(ur => ur.Role.Title)),
                                 AreaUploader = item.UploaderDepartment.Area,
                                 ProvinceUploader = item.UploaderDepartment.Province,
                                 CountyUploader = item.UploaderDepartment.County,
                                 DistrictUploader = item.UploaderDepartment.District,
                                 NationalCodeReceiver = item.ReceiverDepartment.User.UserName,
                                 RoleReceiver = item.ReceiverDepartment.User.UserRoles.Count != 0
                                               ? string.Join("\n", item.ReceiverDepartment.User.UserRoles.Select(ur => ur.Role.Title))
                                               : "-",
                                 AreaReceiver = item.ReceiverDepartment.Area,
                                 ProvinceReceiver = item.ReceiverDepartment.Province,
                                 CountyReceiver = item.ReceiverDepartment.County,
                                 DistrictReceiver = item.ReceiverDepartment.District,
                                 FileFormat = (item.Transfer.FileFormat != null ? item.Transfer.FileFormat : "-"),
                                 UploadDate = item.Transfer.UploadDate.ToShamsi(),
                                 IsActived = (item.Transfer.IsActived) ? "فعال" : "غیرفعال",
                             }).ToList();
            return transfers;
        }

        public List<DisplayTransfersVM> GetMyInboxTransfers(TransferAreaVM area)
        {
            var transfers = _context.DepartmentTransfers.Where(dt => dt.ReceiverDepartment.UserId == area.ReceiverUserId &&
                                                                     dt.IsActived)
                                                         .OrderByDescending(dt => dt.Transfer.UploadDate)
                                                         .Select(dt => new DisplayTransfersVM
                                                         {
                                                             TransferId = dt.Transfer.TransferId,
                                                             Title = dt.Transfer.Title,
                                                             NationalCodeUploader = dt.UploaderDepartment.User.UserName,
                                                             RoleUploader = string.Join("\n", dt.UploaderDepartment.User.UserRoles.Select(ur => ur.Role.Title)),
                                                             AreaUploader = dt.UploaderDepartment.Area,
                                                             ProvinceUploader = dt.UploaderDepartment.Province,
                                                             CountyUploader = dt.UploaderDepartment.County,
                                                             DistrictUploader = dt.UploaderDepartment.District,
                                                             NationalCodeReceiver = dt.ReceiverDepartment.User.UserName,
                                                             RoleReceiver = dt.ReceiverDepartment.User.UserRoles.Count != 0
                                                                               ? string.Join("\n", dt.ReceiverDepartment.User.UserRoles.Select(ur => ur.Role.Title))
                                                                               : "-",
                                                             AreaReceiver = dt.ReceiverDepartment.Area,
                                                             ProvinceReceiver = dt.ReceiverDepartment.Province,
                                                             CountyReceiver = dt.ReceiverDepartment.County,
                                                             DistrictReceiver = dt.ReceiverDepartment.District,
                                                             FileFormat = (dt.Transfer.FileFormat != null ? dt.Transfer.FileFormat : "-"),
                                                             UploadDate = dt.Transfer.UploadDate.ToShamsi(),
                                                             IsActived = (dt.Transfer.IsActived) ? "فعال" : "غیرفعال",
                                                         }).ToList();
            return transfers;

        }

        public List<DisplayTransfersVM> GetMySendTransfers(TransferAreaVM area)
        {
            var transfers = _context.DepartmentTransfers.Where(dt => dt.UploaderDepartment.UserId == area.UploaderUserId)
                                                         .OrderByDescending(dt => dt.Transfer.UploadDate)
                                                         .Select(dt => new DisplayTransfersVM
                                                         {
                                                             TransferId = dt.Transfer.TransferId,
                                                             Title = dt.Transfer.Title,
                                                             NationalCodeUploader = dt.UploaderDepartment.User.UserName,
                                                             RoleUploader = string.Join("\n", dt.UploaderDepartment.User.UserRoles.Select(ur => ur.Role.Title)),
                                                             AreaUploader = dt.UploaderDepartment.Area,
                                                             ProvinceUploader = dt.UploaderDepartment.Province,
                                                             CountyUploader = dt.UploaderDepartment.County,
                                                             DistrictUploader = dt.UploaderDepartment.District,
                                                             NationalCodeReceiver = dt.ReceiverDepartment.User.UserName,
                                                             RoleReceiver = dt.ReceiverDepartment.User.UserRoles.Count != 0
                                                                               ? string.Join("\n", dt.ReceiverDepartment.User.UserRoles.Select(ur => ur.Role.Title))
                                                                               : "-",
                                                             AreaReceiver = dt.ReceiverDepartment.Area,
                                                             ProvinceReceiver = dt.ReceiverDepartment.Province,
                                                             CountyReceiver = dt.ReceiverDepartment.County,
                                                             DistrictReceiver = dt.ReceiverDepartment.District,
                                                             FileFormat = (dt.Transfer.FileFormat != null ? dt.Transfer.FileFormat : "-"),
                                                             UploadDate = dt.Transfer.UploadDate.ToShamsi(),
                                                             IsActived = (dt.Transfer.IsActived) ? "فعال" : "غیرفعال",
                                                         }).ToList();
            return transfers;

        }

    }
}
