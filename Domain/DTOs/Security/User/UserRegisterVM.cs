﻿using Domain.DTOs.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.DTOs.Security.User
{
    public class UserRegisterVM
    {
        public string? UserName { get; set; }
        public int Area {  get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword {  get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public bool Insurance { get; set; }
        public string? EmploymentStatus {  get; set; }
        public IFormFile? Avatar { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? DateOfBirth { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
    }
}