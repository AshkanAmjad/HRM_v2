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
        private readonly IMapper _mapper;

        public UserService(HRMContext context, IUserRepository userRepository, IMapper mapper)
        {
            _context = context;
            _userRepository = userRepository;
            _mapper = mapper;
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
                UploadDocumentToServer(uploadToServer);
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
        public void UploadDocumentToServer(UploadVM document)
        {
            if (document != null)
            {
                DirectionVM direction = _mapper.Map<DirectionVM>(document);
                var path = UploadDirectionOnServer(direction);

                bool dirOrginal = Directory.Exists(path._saveDirOrginal);
                bool dirThumb = Directory.Exists(path._saveDirThumb);

                if (!dirOrginal)
                    Directory.CreateDirectory(path._saveDirOrginal);

                if (!dirThumb)
                    Directory.CreateDirectory(path._saveDirThumb);

                var documentNameOrginal = "";
                var documentNameThumb = "";

                if (document.Name == "Avatar")
                {
                    documentNameOrginal = $"Avatar-{document.UserName}{Path.GetExtension(document.document.FileName)}";
                    documentNameThumb = $"Thumb-{document.UserName}{Path.GetExtension(document.document.FileName)}";
                }
                else
                {
                    documentNameOrginal = $"Document-{document.UserName}{Path.GetExtension(document.document.FileName)}";
                }

                string filePathOriginal = Path.Combine(Directory.GetCurrentDirectory(), path._saveDirOrginal, documentNameOrginal);
                using(var fileStream=new FileStream(filePathOriginal, FileMode.Create))
                    document.document.CopyTo(fileStream);

                if (path._saveDirThumb != "")
                {
                    string filePathThumb = Path.Combine(Directory.GetCurrentDirectory(), path._saveDirThumb, documentNameThumb);
                    ImageConvertor.ResizeImage(filePathOriginal, filePathThumb, 100, 100);
                }

            }
        }

        public DirectionVM UploadDirectionOnServer(DirectionVM direction)
        {
            string saveDirOrginal = "";
            string saveDirThumb = "";

            if (direction.Area == 0)
            {
                if (direction.County == 0 && direction.District == 0)
                {
                    if (direction.Name == "Avatar")
                    {
                        saveDirOrginal = "Areas/Province/Documents/Province/Avatar/Original";
                        saveDirThumb = "Areas/Province/Documents/Province/Avatar/Thumb";

                    }
                    else
                    {
                        saveDirOrginal = "Areas/Province/Documents/Province/Transfer";
                    }
                }
                else if (direction.County != 0 && direction.District == 0)
                {
                    if (direction.Name == "Avatar")
                    {
                        saveDirOrginal = "Areas/Province/Documents/County/Avatar/Original";
                        saveDirThumb = "Areas/Province/Documents/County/Avatar/Thumb";
                    }
                    else
                    {
                        saveDirOrginal = "Areas/Province/Documents/County/Transfer";
                    }
                }
                else
                {
                    if (direction.Name == "Avatar")
                    {
                        saveDirOrginal = "Areas/Province/Documents/District/Avatar/Original";
                        saveDirThumb = "Areas/Province/Documents/District/Avatar/Thumb";
                    }
                    else
                    {
                        saveDirOrginal = "Areas/Province/Documents/District/Transfer";
                    }
                }
            }
            else if (direction.Area == 1)
            {
                if (direction.County == 0 && direction.District == 0)
                {
                    saveDirOrginal = "Areas/County/Documents/County/Transfer";
                }
                else if (direction.County != 0 && direction.District == 0)
                {
                    if (direction.Name == "Avatar")
                    {
                        saveDirOrginal = "Areas/County/Documents/County/Avatar/Original";
                        saveDirThumb = "Areas/County/Documents/County/Avatar/Thumb";

                    }
                    else
                    {
                        saveDirOrginal = "Areas/County/Documents/County/Transfer";
                    }
                }
                else
                {
                    if (direction.County == 0 && direction.District != 0)
                    {
                        if (direction.Name == "Avatar")
                        {
                            saveDirOrginal = "Areas/County/Documents/District/Avatar/Orginal";
                            saveDirThumb = "Areas/County/Documents/District/Avatar/Thumb";
                        }
                        else
                        {
                            saveDirOrginal = "Areas/County/Documents/District/Transfer";
                        }

                    }
                }
            }
            else
            {
                if (direction.County == 0 && direction.District == 0)
                {
                    saveDirOrginal = "Areas/District/Documents/Province/Transfer";
                }
                else if (direction.County != 0 && direction.District == 0)
                {
                    saveDirOrginal = "Areas/District/Documents/County/Transfer";
                }
                else
                {
                    if (direction.Name == "Avatar")
                    {
                        saveDirOrginal = "Areas/District/Documents/Disrrict/Avatar/Original";
                        saveDirThumb = "Areas/District/Documents/Disrrict/Avatar/Thumb";
                    }
                    else
                    {
                        saveDirOrginal = "Areas/District/Documents/Disrrict/Transfer";
                    }
                }
            }

            DirectionVM dir = new();
            dir._saveDirOrginal = saveDirOrginal;
            dir._saveDirThumb = saveDirThumb;
            return dir;
        }




    }


}
