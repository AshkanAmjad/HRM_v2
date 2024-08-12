﻿using Application.Extensions;
using Application.Services.Interfaces;
using AutoMapper;
using Data.Context;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.User;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implrmentations
{
    public class DocumentService:IDocumentService
    {
        #region Constructor
        private readonly IMapper _mapper;

        public DocumentService(HRMContext context, IMapper mapper)
        {
            _mapper = mapper;
        }
        #endregion
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
                using (var fileStream = new FileStream(filePathOriginal, FileMode.Create))
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

        public bool IsExistAvatarOnServer(UserEditVM user)
        {
            bool result= false;
            if(user != null)
            {

                DirectionVM direction = _mapper.Map<DirectionVM>(user);
                var path = UploadDirectionOnServer(direction);

                bool dirOrginal = Directory.Exists(path._saveDirOrginal);
                bool dirThumb = Directory.Exists(path._saveDirThumb);

                bool isExistOrginalAvatar;
                bool isExistThumbAvatar=false;

                if (dirOrginal && dirThumb)
                {
                    var dirOrginalAvatar = $"{path._saveDirOrginal}/Avatar-{user.UserId}.png";
                    isExistOrginalAvatar = Directory.Exists(dirOrginalAvatar);

                    if (isExistOrginalAvatar)
                    {
                        var dirThumbAvatar = $"{path._saveDirThumb}/Thumb-{user.UserId}.png";
                        isExistThumbAvatar = Directory.Exists(dirThumbAvatar);
                    }

                    if (isExistOrginalAvatar && isExistThumbAvatar)
                    {
                       result = true;
                    }
                }
            }
            return result;
        }

    }
}
