using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Security.Login
{
    public class UsernameValidationVM
    {
        public string? Area { get; set; }
        public string? UserName { get; set; }
    }
}
