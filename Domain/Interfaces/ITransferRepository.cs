using Domain.DTOs.General;
using Domain.DTOs.Portal.Transfer;
using Domain.DTOs.Security.User;
using Domain.Entities.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITransferRepository
    {
        #region Transfer
        bool Register(TransferRegisterVM model, out string message);

        #endregion

        #region DB
        void UploadDepartmentTransferToDb(TransferRegisterVM model);
        void UploadTransferToDb(TransferRegisterVM model);
        void SaveChanges();

        #endregion
    }
}
