using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Security.UserRole
{
    public class DisplayDetailsVM
    {
        public string UserName { get; set; }
        public string FullName {  get; set; }
        public List<string> RoleTitle {  get; set; }
        public string AreaDepartment {  get; set; }
        public string ProvinceDepartment {  get; set; }
        public string CountyDepartment { get; set; }
        public string DistrictDepartment { get; set; }
        public string LastActived {  get; set; }
    }
}
