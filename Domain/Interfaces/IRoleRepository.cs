using Domain.DTOs.General;
using Domain.DTOs.Security.Role;
using Domain.DTOs.Security.User;
using Domain.Entities.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRoleRepository
    {
        #region Assistant
        IQueryable<Role> GetAssistantsQuery();
        List<DisplayAssistantsVM> GetAssistants();
        bool RegisterAssistant(AssistantRegisterVM model, out string message);
        bool EditAssistant(AssistantEditVM model, out string message);
        bool DisableAssistant(AssistantEdit_Active_DisableVM model, out string message);
        bool ActiveAssistant(AssistantEdit_Active_DisableVM model, out string message);
        AssistantEditVM? GetAssistantById(Guid roleId);
        bool Similarity(AssistantRegisterVM model, out string message);
        bool Similarity(AssistantEditVM model, out string message);
        #endregion

        #region DB
        void DisableAssistantDb(AssistantEdit_Active_DisableVM model);
        void ActiveAssistantDb(AssistantEdit_Active_DisableVM model);
        void UploadEditAssistantToDb(AssistantEditVM model);
        void UploadRegisterAssistantToDb(AssistantRegisterVM model);
        void SaveChanges();
        #endregion
    }
}
