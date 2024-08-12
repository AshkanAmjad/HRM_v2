using Domain.DTOs.Portal.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDocumentRepository
    {
        #region DB
        void UploadDocumentToDb(UploadVM file);
        void DownloadAvatar(Guid userId);
        #endregion

        #region Is Exist
        bool IsExistAvatarOnDb(Guid userId);
        #endregion
    }
}
