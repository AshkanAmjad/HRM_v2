using AutoMapper;
using Data.Context;
using Data.Extensions;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Transfer;
using Domain.Entities.Portal.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public IQueryable<DepartmentTransfer> GetSendTransfersQuery(AreaVM area)
          => _context.DepartmentTransfers.IgnoreQueryFilters()
                                         .Where(dt => dt.UploaderDepartment.Area == area.Section)
                                         .AsQueryable();


        public List<DisplayTransfersVM> GetSendTransfers(AreaVM area)
        {
            var context = GetSendTransfersQuery(area);
            var transfers = (from item in context 
                             orderby item.Transfer.UploadDate descending
                             select new DisplayTransfersVM
                             {
                                 TransferId = item.Transfer.TransferId,
                                 Title = item.Transfer.Title,
                                 NationalCodeUploader = item.UploaderDepartment.User.UserName,
                                 RoleUploader = string.Join("\n",item.UploaderDepartment.User.UserRoles.Select(ur => ur.Role.Title)),
                                 AreaUploader = item.UploaderDepartment.Area,
                                 ProvinceUploader = item.UploaderDepartment.Province,
                                 CountyUploader = item.UploaderDepartment.County,
                                 DistrictUploader = item.UploaderDepartment.District,
                                 NationalCodeReceiver = item.ReceiverDepartment.User.UserName,
                                 RoleReceiver = string.Join("\n", item.ReceiverDepartment.User.UserRoles.Select(ur => ur.Role.Title)),
                                 AreaReceiver = item.ReceiverDepartment.Area,
                                 ProvinceReceiver = item.ReceiverDepartment.Province,
                                 CountyReceiver = item.ReceiverDepartment.County,
                                 DistrictReceiver = item.ReceiverDepartment.District,
                                 FileFormat = (item.Transfer.FileFormat != null ? item.Transfer.FileFormat : "-"),
                                 UploadDate = item.Transfer.UploadDate.ToShamsiFileUpload(),
                                 IsActived = (item.Transfer.IsActived) ? "فعال" : "غیرفعال",
                             }).ToList();
            return transfers;
        }




    }
}
