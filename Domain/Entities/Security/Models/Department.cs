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
        [Key]
        public Guid DepartmentId { get; set; }
        public Guid UserId { get; set; }
        public int Province { get; set; }
        public int County { get; set; }
        public int District { get; set; }
        public bool IsActived { get; set; }
        public DateTime RegisterDate { get; set; }
        #endregion

        #region Relations
        public virtual ICollection<DepartmentTransfer> DepartmentTransfers { get; set; }
        public virtual ICollection<Document>Documents { get; set; }
        public User User { get; set; }
        #endregion
    }
}
