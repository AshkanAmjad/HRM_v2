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

        public bool Register(UserRegisterVM model, out string message)
        {
            #region Seed Data On Users Table
            model.UserId = Guid.NewGuid();
            model.RegisterDate = DateTime.Now;
            model.LastActived = DateTime.Now;
            model.IsActived = true;
            #endregion 

            #region Hashing Password
            string hashed = Hashing.Main(model.Password);
            model.Password = hashed;
            #endregion

            bool result =  _userRepository.Register(model, out message);

            #region Save Avatar On Server
            if (result && model.Avatar != null)
            {
                UploadVM uploadToServer = _mapper.Map<UploadVM>(model);
                uploadToServer.Name = "Avatar";
                _documentService.UploadDocumentToServer(uploadToServer);
            }
            #endregion

            return result;
        }


        public List<DisplayUsersVM> GetUsers(AreaVM area)
        => _userRepository.GetUsers(area);

        public bool Edit(UserEditVM model, out string message)
        {
            #region Seed Data On Users Table
            model.RegisterDate = DateTime.Now;
            model.LastActived = DateTime.Now;
            #endregion 

            #region Hashing Password
            if (model.Password != null)
            {
                string hashed = Hashing.Main(model.Password);
                model.Password = hashed;
            }
            #endregion

            bool result = _userRepository.Edit(model, out message);

            #region Save Avatar On Server
            if (result && model.Avatar != null)
            {
                UploadVM uploadToServer = _mapper.Map<UploadVM>(model);
                uploadToServer.Name = "Avatar";
                _documentService.UploadDocumentToServer(uploadToServer);
            }
            #endregion

            return result;
        }

        public bool Disable(UserEdit_DisableVM model, out string message)
        {
            bool result = _userRepository.Disable(model, out message);

            #region Delete Document On Server
            if (result)
                _documentService.DeleteDocumentOnServer(model);
            #endregion

            return result;
        }

    }


}
