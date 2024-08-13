using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Portal.Document
{
    public class DownloadAvatarVM
    {
        public byte[]? DataBytes { get; set; }
        public string FileName { get; set; }
    }
}
