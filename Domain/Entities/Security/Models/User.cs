using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Security.Models
{
    [Table("Users", Schema = "Security")]

    public class User
    {
        #region Properties
        [Key]
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Employment { get; set; }
        public string MaritalStatus {  get; set; }
        public bool Insurance {  get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
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
