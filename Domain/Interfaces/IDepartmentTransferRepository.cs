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
        IQueryable<DepartmentTransfer> GetSendTransfersQuery(TransferAreaVM area);
        IQueryable<DepartmentTransfer> GetInboxTransfersQuery(TransferAreaVM area);
        List<DisplayTransfersVM> GetSendTransfers(TransferAreaVM area);
        List<DisplayTransfersVM> GetInboxTransfers(TransferAreaVM area);
        List<DisplayTransfersVM> GetMyInboxTransfers(TransferAreaVM area);
        List<DisplayTransfersVM> GetMySendTransfers(TransferAreaVM area);

        #region Db

        #endregion
    }
}
