﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security.Models
{
    [Table("UserRoles", Schema = "Security")]

    public class UserRole
    {
        #region Properties
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        #endregion

        #region Relations
        public Role Role { get; set; }
        public User User { get; set; }
        #endregion
    }
}