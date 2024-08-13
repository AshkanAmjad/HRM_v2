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
            if (file.document != null && file.document.Length > 0)
            {
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

        }

        public async Task DownloadOrginalAvatar(Document document)
        {
            byte[]? bytes;

            if (document != null)
            {
                DownloadAvatarVM avatar = _mapper.Map<DownloadAvatarVM>(document);

                DirectionVM direction = _mapper.Map<DirectionVM>(document);

                var fileName = avatar.FileName;
                bytes = avatar.DataBytes;

                string? orginalFilePath;

                orginalFilePath = Path.Combine(Directory.GetCurrentDirectory(),direction._saveDirOrginal, $"Avatar-{fileName}");

                using (var fileStream = new FileStream(orginalFilePath, FileMode.Create))
                {
                    await fileStream.WriteAsync(bytes);
                }
            }
        }

        public bool IsExistAvatarOnDb(Guid userId)
            => _context.Documents.Any(d => d.Department.UserId == userId && d.Title == "Avatar" && d.IsActived);

        public Document? GetAvatarWithUserId(Guid userId)
            => _context.Documents.Where(d => d.Department.UserId == userId).SingleOrDefault();

    }
}
