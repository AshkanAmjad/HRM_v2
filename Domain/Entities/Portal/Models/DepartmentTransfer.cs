using Domain.Entities.Security.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Portal.Models
{
    [Table("DepartmentTransfers", Schema = "Portal")]
    public class DepartmentTransfer
    {
        #region Properties
        public Guid DepartmentTransferId {  get; set; }
        public Guid TransferId { get; set; }
        public Guid UploaderDepartmentId { get; set; }
        public Guid ReceiverDepartmentId { get; set; }
        public bool IsActived { get; set; }
        #endregion

        #region Relations
        public virtual Department UploaderDepartment { get; set; }
        public virtual Department ReceiverDepartment { get; set; }
        public virtual Transfer Transfer { get; set; }
        #endregion
    }
}
