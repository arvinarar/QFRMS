using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.DTOs
{
    public class UploadMemo
    {
        public IFormFile? File { get; set; }
    }
}
