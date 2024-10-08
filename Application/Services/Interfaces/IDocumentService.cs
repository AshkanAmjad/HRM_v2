using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.User;
using Domain.Entities.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IDocumentService
    {
        #region Document
        void UploadDocumentToServer(UploadVM document);

        void UploadDocumentToServer(Document document);

        DirectionVM UploadDirectionOnServer(DirectionVM direction);

        void DeleteDocumentOnServer(string filePathOriginal, string filePathThumb);
        void DeleteDocumentOnServer(UserEdit_DisableVM model);
        bool CheckingAvatar(Guid userId, string userName, DirectionVM direction);

        #endregion

        #region Is Exist 
        bool IsExistOrginalAvatarOnServer(DirectionVM direction, string userName);
        bool IsExistThumbAvatarOnServer(DirectionVM direction, string userName);
        

        #endregion
    }
}
