using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Security.Report
{
    public class DisplayHistoriesUsersVM
    {
        public string UserName {  get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public List<string> Roles {  get; set; }
        public string History {  get; set; }
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
