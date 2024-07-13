using Domain.Entities.Security.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Portal.Models
{
    [Table("Documents", Schema = "Portal")]
    public class Document
    {
        #region Properties
        [Key]
        public Guid DocumentId { get; set; }
        public string Title { get; set; }
        public string FileName{get; set; }
        public string? Description { get; set; }
        public string? ContentType { get; set; }
        public string? FileFormat { get; set; }
        public byte[]? DataBytes { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsActived { get; set; }
        public Guid DepartmentId { get; set; }
        #endregion

        #region Relations
        public Department Department { get; set; }
        #endregion
    }
}
