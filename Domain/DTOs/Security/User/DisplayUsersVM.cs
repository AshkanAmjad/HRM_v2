﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Security.User
{
    public class DisplayUsersVM
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string? Area {  get; set; }
        public int? Department {  get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Education {  get; set; }
        public string Employment { get; set; }
        public string MaritalStatus { get; set; }
        public string Insurance { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public bool IsActived { get; set; }
        public DateTime? LastActived { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}