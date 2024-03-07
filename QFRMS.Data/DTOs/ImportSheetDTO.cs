using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.DTOs
{
    public class ImportSheet
    {
        public required bool FromCoursePage { get; set; }
        public required string BatchId { get; set; }

        [Required]
        public IFormFile? File { get; set; }
    }
}
