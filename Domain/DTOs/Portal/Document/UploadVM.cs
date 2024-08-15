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
        public int Area { get; set; }
        public int County {  get; set; }
        public int District {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public IFormFile? document { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
