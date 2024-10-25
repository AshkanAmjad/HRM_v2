using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Portal.Document
{
    public class DisplayMyDocumentsVM
    {
        public Guid DocumentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileFormat { get; set; }
        public string UploadDate { get; set; }
    }
}
