using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Portal.Document
{
    public class DisplayDocumentsVM
    {
        public Guid DocumentId { get; set; }
        public string AreaDepartment { get; set; }
        public string ProvinceDepartment { get; set; }
        public string CountyDepartment { get; set; }
        public string DistrictDepartment { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileFormat { get; set; }
        public string UploadDate { get; set; }
    }
}
