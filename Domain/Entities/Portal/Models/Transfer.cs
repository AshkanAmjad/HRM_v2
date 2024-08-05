using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Portal.Models
{
    [Table("Transfers", Schema = "Portal")]
    public class Transfer
    {
        #region Properties
        public Guid TransferId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ContentType { get; set; }
        public string? FileFormat { get; set; }
        public byte[]? DataBytes { get; set; }
        public Guid UserUploader { get; set; }
        public Guid RoleUploader { get; set; }
        public Guid ReceiverUserId { get; set; }
        public Guid ReceiverRole { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsActived{ get; set; }
        #endregion

        #region Relations
        public virtual ICollection<DepartmentTransfer> DepartmentTransfers { get; set; }
        #endregion
    }
}
