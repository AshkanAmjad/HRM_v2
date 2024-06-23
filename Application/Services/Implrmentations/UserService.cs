﻿using Application.Services.Interfaces;
using Data.Context;
using Domain.DTOs.Enums;
using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.User;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Mvc;

namespace Application.Services.Implrmentations
{
    public class UserService : IUserService
    {
        #region Constructor
        private readonly HRMContext _context;
        private readonly IUserRepository _userRepository;

        public UserService(HRMContext context,IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }
        #endregion
        public List<SelectListItem> GetAreas()
        {
            List<SelectListItem> areas = new()
            {
                new SelectListItem
                {
                    Text = "استان",
                    Value = 0.ToString()
                },
                new SelectListItem
                {
                    Text = "شهرستان",
                    Value = 1.ToString()
                },
                new SelectListItem
                {
                    Text = "بخش",
                    Value = 2.ToString()
                }
            };
            return areas;
        }

        public async Task<User?> GetUser(LoginVM model)
        {
            #region hashing password
            var hasher = new PasswordHasher<string>();
            var password = model.Password;
            var hashedPassword = hasher.HashPassword(null, password);
            model.Password = hashedPassword;
            #endregion

            var user= await _userRepository.GetUser(model);
            return user;
        }

        public bool Register(UserRegisterVM model, out string message)
            => _userRepository.Register(model, out message);



    }
}
