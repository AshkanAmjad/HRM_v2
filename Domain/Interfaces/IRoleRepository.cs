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
        #endregion
    }
}
