using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Security.UserRole
{
    public class UserRoleRegisterVM
    {
        public Guid UserRoleId { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsActived {  get; set; }
    }
}
