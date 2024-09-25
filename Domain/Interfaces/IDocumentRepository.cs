using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.User;
using Domain.Entities.Portal.Models;
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
        Task DownloadOrginalAvatar(Document document);
        void DisableDocumentsDb(Guid departmentId);
        void ActiveDocumentsDb(Guid departmentId);
        Document? GetAvatarWithUserId(Guid userId);
        DirectionVM UploadDirectionOnServer(DirectionVM direction);
        #endregion

        #region Is Exist
        bool IsExistAvatarOnDb(Guid userId);
        #endregion
    }
}
