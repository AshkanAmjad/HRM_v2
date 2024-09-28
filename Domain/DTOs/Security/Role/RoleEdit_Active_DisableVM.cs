using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Security.Role
{
    public class RoleEdit_Active_DisableVM
    {
        public Guid RoleId { get; set; }
        public string? Title {  get; set; }
    }
}
