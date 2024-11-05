using Domain.DTOs.General;
using Domain.DTOs.Portal.Transfer;
using Domain.Entities.Portal.Models;
using Domain.Entities.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDepartmentTransferRepository
    {
        IQueryable<DepartmentTransfer> GetTransfersQuery(AreaVM area);

        List<DisplayTransfersVM> GetTransfers(AreaVM area);

        //List<DisplayDepartmentTransfersVM> GetDepartmentTransfers(string AreaUploader, string AreaReceiver);

        #region Db
        #endregion
    }
}
