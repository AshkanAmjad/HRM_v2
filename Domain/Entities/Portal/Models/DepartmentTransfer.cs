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
        public Guid DepartmentIdUploader { get; set; }
        public Guid DepartmentIdReceiver { get; set; }
        public bool IsActived { get; set; }
        #endregion

        #region Relations
        public Department Department { get; set; }
        public Transfer Transfer { get; set; }
        #endregion
    }
}
