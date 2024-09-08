using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Security.User
{
    public class UserEdit_DeleteVM
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public int? Province {  get; set; }
        public int? County {  get; set; }
        public int? District {  get; set; }
    }
}
