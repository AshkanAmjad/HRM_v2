using AutoMapper;
using Data.Context;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.User;
using Domain.Entities.Portal.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositores
{
    public class DocumentRepository : IDocumentRepository
    {
        #region Constructor
        private readonly HRMContext _context;
        private readonly IMapper _mapper;

        public DocumentRepository(HRMContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion
        public void UploadDocumentToDb(UploadVM file)
        {
            if (file.document == null || file.document.Length <= 0)
                return;

            var userName = file.UserName;
            Document document = _mapper.Map<Document>(file);
            document.IsActived = true;
            document.UploadDate = DateTime.Now;
            document.DocumentId = Guid.NewGuid();
            var docName = Path.GetFileName(file.document.FileName);
            var fileExtension = Path.GetExtension(docName);
            document.FileName = string.Concat($"{file.Name}-{userName}", fileExtension);
            document.FileFormat = fileExtension;
            using (var target = new MemoryStream())
            {
                file.document.CopyTo(target);
                document.DataBytes = target.ToArray();
            }
            _context.Add(document);


        }


        public async Task DownloadOrginalAvatar(Document document)
        {
            if (document == null)
                return;

            byte[]? bytes;

            var avatar = _mapper.Map<DownloadAvatarVM>(document);
            bytes = avatar.DataBytes;

            DirectionVM direction;
            direction = _mapper.Map<DirectionVM>(document);
            direction.Name = "Avatar";

            var path = UploadDirectionOnServer(direction);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), path._saveDirOrginal, avatar.FileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await fileStream.WriteAsync(bytes);
        }

        public bool IsExistAvatarOnDb(Guid userId)
            => _context.Documents.Any(d => d.Department.UserId == userId && d.Title == "Avatar" && d.IsActived);

        public Document? GetAvatarWithUserId(Guid userId)
            => _context.Documents.Include(d => d.Department).ThenInclude(d=>d.User).Where(d => d.Department.UserId == userId).SingleOrDefault();

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
