using AutoMapper;
using Data.Context;
using Data.Extensions;
using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.User;
using Domain.Entities.Portal.Models;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositores
{
    public class DocumentRepository : IDocumentRepository
    {
        #region Constructor
        private readonly HRMContext _context;
        private readonly IMapper _mapper;


        public DocumentRepository(
            HRMContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion
        public void UploadDocumentToDb(UploadVM file)
        {
            if (file.Document == null || file.Document.Length <= 0)
                return;

            var userName = file.Title;
            Document document = _mapper.Map<Document>(file);
            document.IsActived = true;
            document.UploadDate = DateTime.Now;
            document.DocumentId = Guid.NewGuid();
            var docName = Path.GetFileName(file.Document.FileName);
            var fileExtension = Path.GetExtension(docName);
            document.FileName = string.Concat($"{file.Name}-{userName}", fileExtension);
            document.FileFormat = fileExtension;
            using (var target = new MemoryStream())
            {
                file.Document.CopyTo(target);
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
        => _context.Documents.Any(d => d.Department.UserId == userId && d.Title == "Avatar");



        public Document? GetAvatarByUserId(Guid userId)
            => _context.Documents.Include(d => d.Department).ThenInclude(d => d.User).Where(d => d.Department.UserId == userId).SingleOrDefault();

        public DirectionVM UploadDirectionOnServer(DirectionVM direction)
        {
            string saveDirOrginal = "";
            string saveDirThumb = "";

            if (direction.Area == "0")
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
                else if (direction.County != "0" && direction.District == "0")
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
            else if (direction.Area == "1")
            {
                if (direction.County == "0" && direction.District == "0")
                {
                    saveDirOrginal = "Areas/County/Documents/County/Transfers";
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

        public void DisableDocumentsDb(Guid departmentId)
        {

            if (departmentId != Guid.Empty)
            {

                var documents = _context.Documents.Where(d => d.DepartmentId == departmentId)
                                                  .ToList();

                if (documents.Count == 0)
                {
                    return;
                }

                foreach (var document in documents)
                {
                    document.IsActived = false;
                }

                _context.Documents.UpdateRange(documents);

            }
        }

        public void ActiveDocumentsDb(Guid departmentId)
        {
            if (departmentId == Guid.Empty)
            {
                return;
            }

            var documents = _context.Documents.IgnoreQueryFilters()
                                              .Where(d => d.DepartmentId == departmentId)
                                              .ToList();

            if (documents.Count == 0)
            {
                return;
            }

            foreach (var document in documents)
            {
                document.IsActived = true;
            }

            _context.Documents.UpdateRange(documents);
        }

        public IQueryable<Document> GetDocumentsQuery(AreaVM area)
        {
            IQueryable<Document> context;

            if (area.Section == "0")
            {
                context = _context.Documents.IgnoreQueryFilters()
                                            .Where(d => d.Department.Area == area.Display)
                                            .AsQueryable();
            }
            else if (area.Section == "1")
            {
                context = _context.Documents.IgnoreQueryFilters()
                                            .Where(d => d.Department.Area == area.Display &&
                                                        d.Department.Province == area.Province &&
                                                        d.Department.County == area.County)
                                            .AsQueryable();
            }
            else
            {
                context = _context.Documents.IgnoreQueryFilters()
                                            .Where(d => d.Department.Area == area.Display &&
                                                        d.Department.Province == area.Province &&
                                                        d.Department.County == area.County &&
                                                        d.Department.District == area.District)
                                            .AsQueryable();
            }
            return context;

        }

        public List<DisplayDocumentsVM> GetDocuments(AreaVM area, bool status)
        {
            var context = GetDocumentsQuery(area);
            var documents = (from item in context
                             where (item.IsActived == status)
                             orderby item.UploadDate descending
                             select new DisplayDocumentsVM
                             {
                                 DocumentId = item.DocumentId,
                                 UserName = item.Department.User.UserName,
                                 UploadDate = $"{item.UploadDate.ToShamsi()}",
                                 AreaDepartment = (item.Department.Area == "0" ? "استان" : (item.Department.Area == "1" ? "شهرستان" : "بخش")),
                                 CountyDepartment = item.Department.County,
                                 ProvinceDepartment = item.Department.Province,
                                 DistrictDepartment = item.Department.District,
                                 Description = item.Description,
                                 FileFormat = item.FileFormat,
                                 Title = (item.Title == "Avatar" ? "تصویر پروفایل" : item.Title),
                                 FirstName = item.Department.User.FirstName,
                                 LastName = item.Department.User.LastName
                             })
                                    .ToList();
            return documents;
        }

        public bool IsExistDocumentOnDb(Guid documentId)
            => _context.Documents.IgnoreQueryFilters()
                                 .Where(d => d.DocumentId == documentId)
                                 .Any();

        public DownloadDocumentVM? GetDocumentById(Guid documentId)
        {
            DownloadDocumentVM document = new();
            if (documentId != Guid.Empty)
            {
                document = _context.Documents.IgnoreQueryFilters()
                                             .Where(d => d.DocumentId == documentId)
                                             .Select(d => new DownloadDocumentVM
                                             {
                                                 Bytes = d.DataBytes,
                                                 ContentType = d.ContentType,
                                                 FileName = d.FileName
                                             })
                                             .FirstOrDefault();
            }
            return document;
        }

        public List<DisplayMyDocumentsVM> GetMyDocuments(AreaVM area, Guid userId)
        {
            var documents = _context.Documents.Where(d => d.Department.UserId == userId &&
                                                          d.Department.Area == area.Section)
                                              .Select(d => new DisplayMyDocumentsVM
                                              {
                                                  DocumentId = d.DocumentId,
                                                  Description = d.Description,
                                                  Title = (d.Title == "Avatar" ? "تصویر پروفایل" : d.Title),
                                                  FileFormat = d.FileFormat,
                                                  UploadDate = $"{d.UploadDate.ToShamsi()}"
                                              }).ToList();
            return documents;
        }




    }
}
