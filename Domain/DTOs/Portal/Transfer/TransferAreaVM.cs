using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Portal.Transfer
{
    public class TransferAreaVM
    {
        public string? UploaderArea { get; set; }
        public Guid UploaderUserId {  get; set; }
        public string? UploaderProvince {  get; set; }
        public string? UploaderCounty { get; set; }
        public string? UploaderDistrict { get; set; }
        public string? ReceiverArea { get; set; }
        public string? ReceiverProvince { get; set; }
        public string? ReceiverCounty { get; set; }
        public string? ReceiverDistrict { get; set; }
        public bool Display {  get; set; }
        public Guid ReceiverUserId {  get; set; }
    }
}
