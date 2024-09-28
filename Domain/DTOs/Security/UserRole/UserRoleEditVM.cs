using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Security.UserRole
{
    public class UserRoleEditVM
    {
        public Guid UserRolesId { get; set; }
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
