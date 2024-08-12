using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.User;
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
        DirectionVM UploadDirectionOnServer(DirectionVM direction);
        #endregion

        #region Is Exist 
        bool IsExistAvatarOnServer(UserEditVM user);

        #endregion
    }
}
