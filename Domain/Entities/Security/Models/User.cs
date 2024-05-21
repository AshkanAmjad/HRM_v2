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
    [Table("Users", Schema = "Security")]

    public class User: BaseEntity
    {
        #region Properties
        [Key]
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus {  get; set; }
        public string? Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public bool IsActived { get; set; }
        public DateTime? LastActived { get; set; }
        public DateTime RegisterDate { get; set; }
        #endregion

        #region Relations
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public Department Department { get; set; }
        #endregion

    }
}
