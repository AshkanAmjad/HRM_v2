﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Portal.Transfer
{
    public class DisplayDepartmentTransfersVM
    {
        public Guid TransferId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ContentType { get; set; }
        public string? FileFormat { get; set; }
        public byte[]? DataBytes { get; set; }
        public string AreaUploader {  get; set; }
        public string NationalCodeUploader { get; set; }
        public string RoleUploader { get; set; }
        public string ProvinceUploader {  get; set; }
        public string CountyUploader { get; set; }
        public string DistrictUploader { get; set; }
        public string NationalCodeReceiver { get; set; }
        public string AreaReceiver { get; set; }
        public string RoleReceiver { get; set; }
        public string ProvinceReceiver { get; set; }
        public string CountyReceiver { get; set; }
        public string DistrictReceiver { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsActived { get; set; }
    }
}
