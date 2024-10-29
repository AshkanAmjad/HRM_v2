using AutoMapper;
using Data.Context;
using Domain.DTOs.Portal.Transfer;
using Domain.Entities.Portal.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositores
{
    public class DepartmentTransferRepository:IDepartmentTransferRepository
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
        //public IQueryable<DepartmentTransfer> GetInternalDepartmentTransfersQuery(string AreaUploader, string AreaReceiver)
        //    => _context.DepartmentTransfers.IgnoreQueryFilters()
        //                                   .Where(dt => dt.Department == AreaReceiver && dt.Department.Area == AreaUploader)
        //                                   .Include(dt => dt.Department)
        //                                   .Include(dt => dt.Transfer)
        //                                   .AsQueryable();
        //public List<DisplayDepartmentTransfersVM> GetDepartmentTransfers(string AreaUploader, string AreaReceiver)
        //{
        //    var context = GetInternalDepartmentTransfersQuery(AreaUploader, AreaReceiver);

        //    var transfers = (from item in context
        //                     orderby item.IsActived descending, item.Transfer.UploadDate descending
        //                     select new DisplayDepartmentTransfersVM
        //                     {
        //                         TransferId = item.Transfer.TransferId,
        //                         NationalCodeReceiver = item.Department.User.UserName,
        //                         NationalCodeUploader = item.Department.User.UserName,
        //                         AreaReceiver = item.Department.Area,
        //                         AreaUploader = item.Department.Area,

        //                     })
        //                     .ToList();
        //    return transfers;
        //}
    }
}
