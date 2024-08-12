using Application.Extensions;
using Application.Services.Interfaces;
using AutoMapper;
using Data.Context;
using Data.Repositores;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.User;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Web.Mvc;

namespace Application.Services.Implrmentations
{
    public class UserService : IUserService
    {
        #region Constructor
        private readonly HRMContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper, IDocumentService documentService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _documentService = documentService;
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

        public async Task<User?> GetUserAsync(LoginVM model)
        {
            //#region hashing password
            //var hasher = new PasswordHasher<string>();
            //var password = model.Password;
            //var hashedPassword = hasher.HashPassword(null, password);
            //model.Password = hashedPassword;
            //#endregion

            string hashed = Hashing(model.Password);
            model.Password = hashed;
            var user = await _userRepository.GetUserAsync(model);
            return user;
        }

        public bool Register(UserRegisterVM model, out string message)
        {
            #region Seed Data On Users Table
            model.UserId = Guid.NewGuid();
            model.RegisterDate = DateTime.Now;
            model.LastActived = DateTime.Now;
            model.IsActived = true;
            #endregion 

            #region Hashing Password
            string hashed = Hashing(model.Password);
            model.Password = hashed;
            #endregion

            #region Save Avatar On Server
            if (model.Avatar != null && !_userRepository.Similarity(model, out message))
            {
                UploadVM uploadToServer = _mapper.Map<UploadVM>(model);
                uploadToServer.Name = "Avatar";
                _documentService.UploadDocumentToServer(uploadToServer);
            }
            #endregion

            return _userRepository.Register(model, out message);
        }

        public string Hashing(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: password,
                            salt: salt,
                            prf: KeyDerivationPrf.HMACSHA256,
                            iterationCount: 100000,
                            numBytesRequested: 256 / 8
            ));
            return hashed;
        }
        public List<DisplayUsersVM> GetUsers(AreaVM area)
        => _userRepository.GetUsers(area);

    }


}
