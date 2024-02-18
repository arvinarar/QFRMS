using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.DTOs
{
    public class UpdateBatch : CreateBatch
    {
        public required string Id { get; set; }
        public required string NTPId { get; set; }
        [DisplayName("NTP : ")]
        public new IFormFile? NTP { get; set; }
        public string? CertificatesId { get; set; }
        public string? CoursePageId { get; set; }
        [DisplayName("Replace Current NTP?")]
        public required bool OverwriteNTP { get; set; }
        [DisplayName("Delete Current Certificates?")]
        public required bool OverwriteCertificate { get; set; }
    }
}
