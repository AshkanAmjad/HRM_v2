using Application.Extensions;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.Profile;
using Domain.DTOs.Security.User;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public bool VerificationCode(VerificationCodeVM model, string code, out string message)
        {
            bool result = false;
            string checkMessage = "کد وارد شده با کد ارسال شده تطابق نمی کند.";

            if (model.Code.Equals(code))
            {
                result = true;
                checkMessage = "";
            }

            message = checkMessage;
            return result;
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
            string hashed = Hashing.Hash(model.Password);
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

        public bool ResetPassword(ResetPasswordVM model, out string message)
        {
            #region Seed Data On Users Table
            model.LastActived = DateTime.Now;
            #endregion

            #region Hashing Password
            string hashed = Hashing.Hash(model.Password);
            model.Password = hashed;
            #endregion

            bool result = _userRepository.ResetPassword(model, out message);

            return result;
        }


        public bool Edit(UserEditVM model, out string message)
        {

            #region Hashing Password
            if (model.Password != null)
            {
                string hashed = Hashing.Hash(model.Password);
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

        public bool Edit(ProfileEditVM model, out string message)
        {
            #region Hashing Password
            if (model.Password != null)
            {
                string hashed = Hashing.Hash(model.Password);
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

        public bool Active(UserDelete_ActiveVM model, out string message)
        {
            bool result = _userRepository.Active(model, out message);

            return result;
        }
    }


}
