using Domain.Entities.Security.Models;
using Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Portal.Models
{
    [Table("DepartmentTransfers", Schema = "Portal")]
    public class DepartmentTransfer:BaseEntity
    {
        #region Properties
        public Guid TransferId { get; set; }
        public Guid DepartmentId { get; set; }
        #endregion

        #region Relations
        public Department Department { get; set; }
        public Transfer Transfer { get; set; }
        #endregion
    }
}
