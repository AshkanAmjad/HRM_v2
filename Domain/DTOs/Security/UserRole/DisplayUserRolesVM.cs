using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Security.UserRole
{
    public class DisplayUserRolesVM
    {
        public Guid UserRoleId { get; set; }
        public string Title {  get; set; }
        public string UserName {  get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Area {  get; set; }
        public string County { get; set; }
        public string District { get; set; }
        public string IsActived { get; set; }
        public string RegisterDate { get; set; }
    }
}
