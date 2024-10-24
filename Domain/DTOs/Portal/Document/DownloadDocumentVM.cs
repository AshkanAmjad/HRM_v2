using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Portal.Document
{
    public class DownloadDocumentVM
    {
        public byte[] Bytes {  get; set; }
        public string ContentType {  get; set; }
        public string FileName {  get; set; }
    }
}
