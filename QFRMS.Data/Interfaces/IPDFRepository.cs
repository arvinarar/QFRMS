using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Interfaces
{
    public interface IPDFRepository
    {
        public Task<IQueryable<PDF>> RetrieveAllAsync();
        public Task<PDF> GetPDF(string id);
        public Task<FileContentResult> GetPDFFile(string id);
        public Task<PDF> CreatePDF(string Id, string pdfName, IFormFile pdfFile);
        public Task<bool> UpdatePDFFile(string Id, IFormFile pdfFile);
        public Task<bool> UpdatePDF(PDF model);
        public Task<bool> DeletePDF(string Id);
    }
}
