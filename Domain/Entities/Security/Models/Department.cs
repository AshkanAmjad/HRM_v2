using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Portal.Models;

namespace Domain.Entities.Security.Models
{
    [Table("Departments", Schema = "Security")]
    public class Department
    {
        #region Properties
        public Guid DepartmentId { get; set; }
        public Guid UserId { get; set; }
        public string Area {  get; set; }
        public string Province { get; set; }
        public string County { get; set; }
        public string District { get; set; }
        public bool IsActived { get; set; }
        public DateTime RegisterDate { get; set; }
        #endregion

        #region Relations
        public virtual ICollection<DepartmentTransfer> UploaderTransfers { get; set; }
        public virtual ICollection<DepartmentTransfer> ReceiverTransfers { get; set; }
        public virtual ICollection<Document>Documents { get; set; }
        public virtual User User { get; set; }
        #endregion
    }
}
