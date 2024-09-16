using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Security.User
{
    public class UserEdit_DisableVM
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? area {  get; set; }
        public string? Province {  get; set; }
        public string? County {  get; set; }
        public string? District {  get; set; }
    }
}
