using Application.Extensions;
using Application.Services.Interfaces;
using AutoMapper;
using Data.Context;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.User;
using Domain.Entities.Portal.Models;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implrmentations
{
    public class DocumentService : IDocumentService
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly IDocumentRepository _documentRepository;

        public DocumentService(HRMContext context, IMapper mapper, IDocumentRepository documentRepository)
        {
            _mapper = mapper;
            _documentRepository = documentRepository;
        }

        #endregion
        public bool CheckingAvatar(Guid userId, string userName, DirectionVM direction)
        {
            bool IsExistAvatarOnDb = _documentRepository.IsExistAvatarOnDb(userId);

            if (IsExistAvatarOnDb)
            {

                bool isExistOrginalAvatar = IsExistOrginalAvatarOnServer(direction, userName);

                bool isExistThumbAvatar = IsExistThumbAvatarOnServer(direction, userName);

                if (!isExistOrginalAvatar || !isExistThumbAvatar)
                {
                    var avatar = _documentRepository.GetAvatarByUserId(userId);

                    if (!isExistOrginalAvatar)
                        _documentRepository.DownloadOrginalAvatar(avatar);

                    if (!isExistThumbAvatar)
                        UploadDocumentToServer(avatar);
                }

            }

            return IsExistAvatarOnDb;

        }

        public void UploadDocumentToServer(UploadVM document)
        {
            if (document == null)
                return;

            DirectionVM direction = _mapper.Map<DirectionVM>(document);
            var path = UploadDirectionOnServer(direction);

            bool dirOrginal = Directory.Exists(path._saveDirOrginal);
            bool dirThumb = Directory.Exists(path._saveDirThumb);

            if (!dirOrginal)
                Directory.CreateDirectory(path._saveDirOrginal);

            if (!dirThumb && document.Name == "Avatar")
                Directory.CreateDirectory(path._saveDirThumb);

            var documentNameOrginal = "";
            var documentNameThumb = "";

            if (document.Name == "Avatar")
            {
                documentNameOrginal = $"Avatar-{document.Title}{Path.GetExtension(document.Document.FileName)}";
                documentNameThumb = $"Thumb-{document.Title}{Path.GetExtension(document.Document.FileName)}";
            }
            else
            {
                documentNameOrginal = $"Document-{document.Title}{Path.GetExtension(document.Document.FileName)}";
            }

            string filePathOriginal = Path.Combine(Directory.GetCurrentDirectory(), path._saveDirOrginal, documentNameOrginal);

            DeleteDocumentOnServer(filePathOriginal, "");

            using (var fileStream = new FileStream(filePathOriginal, FileMode.Create))
            {
                document.Document.CopyTo(fileStream);
            }

            if (path._saveDirThumb != "")
            {
                string filePathThumb = Path.Combine(Directory.GetCurrentDirectory(), path._saveDirThumb, documentNameThumb);

                string watermarkImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/icons/general/watermark", "logoWatermark.png");

                DeleteDocumentOnServer("", filePathThumb);

                SKBitmap resizedBitmap = ImageConverter.ResizeImage(filePathOriginal, 200, 200 );
                SKBitmap watermarkedBitmap = ImageConverter.WatermarkImage(resizedBitmap, watermarkImagePath);
                ImageConverter.SaveImage(watermarkedBitmap, filePathThumb, 100);
            }
        }

        public void UploadDocumentToServer(Document document)
        {
            if (document == null)
                return;

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

            if (document.Title == "Avatar")
            {
                documentNameOrginal = $"Avatar-{document.Department.User.UserName}{Path.GetExtension(document.FileFormat)}";
                documentNameThumb = $"Thumb-{document.Department.User.UserName}{Path.GetExtension(document.FileFormat)}";
            }
            else
            {
                documentNameOrginal = $"Document-{document.Department.User.UserName}{Path.GetExtension(document.FileFormat)}";
            }

            byte[]? bytes = document.DataBytes;

            string filePathOriginal = Path.Combine(Directory.GetCurrentDirectory(), path._saveDirOrginal, documentNameOrginal);

            int retryCount = 0;
            while (retryCount < 5)
            {
                try
                {
                    using (var fileStream = new FileStream(filePathOriginal, FileMode.Truncate))
                    {
                        fileStream.Write(bytes);
                        break;
                    }
                }
                catch (IOException ex)
                {
                    if (ex.Message.Contains("The process cannot access the file"))
                    {
                        Console.WriteLine("File is locked. Waiting for 1 second...");
                        Thread.Sleep(1000);
                        retryCount++;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            if (path._saveDirThumb != "")
            {
                string filePathThumb = Path.Combine(Directory.GetCurrentDirectory(), path._saveDirThumb, documentNameThumb);
                string watermarkImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/icons/general/watermark", "logoWatermark.png");

                SKBitmap resizedBitmap = ImageConverter.ResizeImage(filePathOriginal, 200, 200);
                SKBitmap watermarkedBitmap = ImageConverter.WatermarkImage(resizedBitmap, watermarkImagePath);
                ImageConverter.SaveImage(watermarkedBitmap, filePathThumb, 100);
            }
        }


        public DirectionVM UploadDirectionOnServer(DirectionVM direction)
        {
            string saveDirOrginal = "";
            string saveDirThumb = "";

            if (direction.Area == "Province" || direction.Area == "0") 
            {
                if (direction.County == "0" && direction.District == "0")
                {
                    if (direction.Name == "Avatar")
                    {
                        saveDirOrginal = "Areas/Province/Documents/Province/Avatars/Original";
                        saveDirThumb = "Areas/Province/Documents/Province/Avatars/Thumb";

                    }
                    else
                    {
                        saveDirOrginal = "Areas/Province/Documents/Province/Transfers";
                    }
                }
                else if ( direction.County != "0" && direction.District == "0")
                {
                    if (direction.Name != "Avatar")
                    {
                        saveDirOrginal = "Areas/Province/Documents/County/Transfers";
                    }
                }
                else
                {
                    if (direction.Name != "Avatar")
                    {
                        saveDirOrginal = "Areas/Province/Documents/District/Transfers";
                    }
                }
            }
            else if (direction.Area == "County" || direction.Area == "1")
            {
                if (direction.County == "0" && direction.District == "0")
                {
                    saveDirOrginal = "Areas/County/Documents/Province/Transfers";
                }
                else if (direction.County != "0" && direction.District == "0")
                {
                    if (direction.Name == "Avatar")
                    {
                        saveDirOrginal = "Areas/County/Documents/County/Avatars/Original";
                        saveDirThumb = "Areas/County/Documents/County/Avatars/Thumb";

                    }
                    else
                    {
                        saveDirOrginal = "Areas/County/Documents/County/Transfers";
                    }
                }
                else
                {
                    if (direction.County != "0" && direction.District != "0")
                    {
                        if (direction.Name != "Avatar")
                        {
                            saveDirOrginal = "Areas/County/Documents/District/Transfers";
                        }

                    }
                }
            }
            else
            {
                if (direction.County == "0" && direction.District == "0")
                {
                    saveDirOrginal = "Areas/District/Documents/Province/Transfers";
                }
                else if (direction.County != "0" && direction.District == "0")
                {
                    saveDirOrginal = "Areas/District/Documents/County/Transfers";
                }
                else
                {
                    if (direction.Name == "Avatar")
                    {
                        saveDirOrginal = "Areas/District/Documents/District/Avatars/Original";
                        saveDirThumb = "Areas/District/Documents/District/Avatars/Thumb";
                    }
                    else
                    {
                        saveDirOrginal = "Areas/District/Documents/District/Transfers";
                    }
                }
            }

            DirectionVM dir = new();
            dir._saveDirOrginal = saveDirOrginal;
            dir._saveDirThumb = saveDirThumb;
            return dir;
        }

        public bool IsExistOrginalAvatarOnServer(DirectionVM direction, string userName)
        {
            bool result = false;

            if (direction != null && userName != "")
            {
                direction.Name = "Avatar";

                var path = UploadDirectionOnServer(direction);

                bool dirOrginal = Directory.Exists(path._saveDirOrginal);

                bool isExistOrginalAvatar;

                if (dirOrginal)
                {
                    var dirOrginalAvatar = $"{path._saveDirOrginal}/Avatar-{userName}{Path.GetExtension(".png")}";
                    isExistOrginalAvatar = File.Exists(dirOrginalAvatar);

                    if (isExistOrginalAvatar)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool IsExistThumbAvatarOnServer(DirectionVM direction, string userName)
        {
            bool result = false;

            if (direction != null && userName != "")
            {
                direction.Name = "Avatar";

                var path = UploadDirectionOnServer(direction);

                bool dirThumb = Directory.Exists(path._saveDirThumb);

                bool isExistThumbAvatar = false;

                if (dirThumb)
                {
                    var dirThumbAvatar = $"{path._saveDirThumb}/Thumb-{userName}{Path.GetExtension(".png")}";
                    isExistThumbAvatar = File.Exists(dirThumbAvatar);
                }

                if (isExistThumbAvatar)
                {
                    result = true;
                }
            }
            return result;
        }


        public void DeleteDocumentOnServer(string filePathOriginal, string filePathThumb)
        {
            if (filePathOriginal != "" && File.Exists(filePathOriginal))
                File.Delete(filePathOriginal);

            if (filePathThumb != "" && File.Exists(filePathThumb))
                File.Delete(filePathThumb);
        }

        public void DeleteDocumentOnServer(UserEdit_DisableVM model)
        {
            if (model != null)
            {
                DirectionVM direction;
                direction = _mapper.Map<DirectionVM>(model);
                direction.Name = "Avatar";

                DirectionVM path;

                path = UploadDirectionOnServer(direction);

                var documentNameOrginal = "";
                var documentNameThumb = "";

                if (direction.Name == "Avatar")
                {
                    documentNameOrginal = $"Avatar-{model.UserName}{Path.GetExtension(".png")}";
                    documentNameThumb = $"Thumb-{model.UserName}{Path.GetExtension(".png")}";
                }

                string avatarPathOriginal = Path.Combine(Directory.GetCurrentDirectory(), path._saveDirOrginal, documentNameOrginal);

                string avatarPathThumb = "";

                if (path._saveDirThumb != "")
                {
                    avatarPathThumb = Path.Combine(Directory.GetCurrentDirectory(), path._saveDirThumb, documentNameThumb);
                }

                if (avatarPathOriginal != null || avatarPathThumb != null)
                    DeleteDocumentOnServer(avatarPathOriginal, avatarPathThumb);


                direction.Name = "Document";

                path = UploadDirectionOnServer(direction);

                if (direction.Name != "Avatar")
                {
                    documentNameOrginal = Path.GetFileNameWithoutExtension($"Document-{model.UserName}");
                }

                string filePathOriginal = Path.Combine(Directory.GetCurrentDirectory(), path._saveDirOrginal, documentNameOrginal);

                DeleteDocumentOnServer(filePathOriginal, "");
            }

        }

    }
}
