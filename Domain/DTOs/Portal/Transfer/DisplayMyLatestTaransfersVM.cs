using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Portal.Transfer
{
    public class DisplayMyLatestTaransfersVM
    {
        public Guid TransferId { get; set; }
        public string Title { get; set; }
        public string AreaUploader { get; set; }
        public string NationalCodeUploader { get; set; }
        public string RoleUploader { get; set; }
        public string ProvinceUploader { get; set; }
        public string CountyUploader { get; set; }
        public string DistrictUploader { get; set; }
        public string? FileFormat { get; set; }
        public string UploadDate { get; set; }
    }
}
