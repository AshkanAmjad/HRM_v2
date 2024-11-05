using AutoMapper;
using Data.Context;
using Domain.DTOs.General;
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

        public IQueryable<DepartmentTransfer> GetTransfersQuery(AreaVM area)
                  => _context.DepartmentTransfers.Include(dt => dt.Transfer)
                                                 .Include(dt => dt.Department)
                                                 .Where(dt => dt.Department.Area == area.Section)
                                                 .AsQueryable();

        public List<DisplayTransfersVM> GetTransfers(AreaVM area)
        {
            throw new NotImplementedException();
        }

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
