using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Portal.Transfer;
using Domain.DTOs.Security.User;
using Domain.Entities.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITransferRepository
    {
        #region Transfer
        bool Register(TransferRegisterVM model, out string message);
        bool Disable(TransferActive_Disable_DescriptionVM model, out string message);
        bool Active(TransferActive_Disable_DescriptionVM model, out string message);
        bool ActivingDisplay(TransferActive_Disable_DescriptionVM model, out string message);
        GetDescriptionVM GetDescription(TransferActive_Disable_DescriptionVM model);
        #endregion

        #region DB
        void UploadDepartmentTransferToDb(TransferRegisterVM model);
        void UploadTransferToDb(TransferRegisterVM model);
        DownloadDocumentVM? GetTransferById(Guid transferId);
        void ActiveTransferDb(TransferActive_Disable_DescriptionVM model);
        void DisableTransferDb(TransferActive_Disable_DescriptionVM model);
        void ActiveDisplayTransferDb(TransferActive_Disable_DescriptionVM model);
        void SaveChanges();
        #endregion

        #region Is Exist
        bool IsExistTransferOnDb(Guid transferId);

        #endregion
    }
}
