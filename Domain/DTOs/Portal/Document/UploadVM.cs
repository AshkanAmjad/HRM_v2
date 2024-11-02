using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Portal.Document
{
    public class UploadVM
    {
        public string? Area { get; set; }
        public string? County {  get; set; }
        public string? District {  get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Title { get; set; }
        public IFormFile? Document { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
