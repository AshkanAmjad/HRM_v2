using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Security.Role
{
    public class DisplayRolesVM
    {
        public Guid RoleId { get; set; }
        public string Title {  get; set; }
        public string IsActived { get; set; }
        public string RegisterDate { get; set; }

    }
}
