using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPermissionRepository
    {
        #region Permission
        bool CheckAreaPermission(string area, Guid userId);
        bool CheckRolePermission(string[] roleTitles, Guid userId);
        List<string> GetUserRolesById(Guid userId);
        #endregion
    }
}
