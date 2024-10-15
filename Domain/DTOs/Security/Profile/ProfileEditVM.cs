using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Security.Profile
{
    public class ProfileEditVM
    {
        public Guid UserId { get; set; }
        public Guid DepartmenyId { get; set; }
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Education { get; set; }
        public string? MaritalStatus { get; set; }
        public IFormFile? Avatar { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? AreaDepartment { get; set; }
        public string? CountyDepartment { get; set; }
        public string? DistrictDepartment { get; set; }
        public DateTime LastActived { get; set; }

    }
}
