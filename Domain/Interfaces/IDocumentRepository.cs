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
        #region Documents
        IQueryable<Document> GetDocumentsQuery();
        List<DisplayDocumentsVM> GetDocuments(AreaVM area, bool status);
        List<DisplayMyDocumentsVM> GetMyDocuments(AreaVM area, Guid userId);
        #endregion

        #region DB        
        void UploadDocumentToDb(UploadVM file);
        Task DownloadOrginalAvatar(Document document);
        void DisableDocumentsDb(Guid departmentId);
        void ActiveDocumentsDb(Guid departmentId);
        Document? GetAvatarByUserId(Guid userId);
        DownloadDocumentVM? GetDocumentById(Guid documentId);
        DirectionVM UploadDirectionOnServer(DirectionVM direction);
        #endregion

        #region Is Exist
        bool IsExistAvatarOnDb(Guid userId);
        bool IsExistDocumentOnDb(Guid documentId);
        #endregion
    }
}
