using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Security.UserRole
{
    public class UserRolesDirectionVM
    {
        public string? Level {  get; set; }
        public string? Province {  get; set; }
        public string? County {  get; set; }
        public string? District { get; set; }
    }
}
