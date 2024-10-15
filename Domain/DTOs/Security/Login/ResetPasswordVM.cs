using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Security.Login
{
    public class ResetPasswordVM
    {
        public Guid UserId { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastActived { get; set; }
    }
}
