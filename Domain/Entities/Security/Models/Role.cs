using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces.Base;

namespace Domain.Entities.Security.Models
{
    [Table("Roles", Schema = "Security")]
    public class Role:BaseEntity
    {
        #region Properties
        [Key]
        public Guid RoleId { get; set; }
        public string Title { get; set; }
        #endregion

        #region Relations
        public virtual ICollection<UserRole> UserRoles { get; set; }
        #endregion
    }
}
