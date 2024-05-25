using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Security.Login
{
    public class LoginVM
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Area { get; set; }
        public bool RememberMe {  get; set; }

    }
}
