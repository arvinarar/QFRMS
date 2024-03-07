using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QFRMS.Data.Interfaces;
using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Repositories
{
    public class PDFRepository : IPDFRepository
    {
        public readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly ILogger<PDFRepository> _logger;

        public PDFRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<PDFRepository> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<PDF> GetPDF(string id)
        {
            try
            {
                return await _context.PDFs.FindAsync(id) ?? throw new NullReferenceException("Database: PDF not found");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FileContentResult> GetPDFFile(string id)
        {
            try
            {
                var pdf = await GetPDF(id);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "PDFs");
                string pdfPath = Path.Combine(path, pdf.FilePath);
                if (!File.Exists(pdfPath)) throw new NullReferenceException("File Not Found");
                return new FileContentResult(File.ReadAllBytes(pdfPath), "application/pdf");
            }
            catch(Exception) 
            {
                throw;
            }
        }

        public async Task<PDF> CreatePDF(string Id, string pdfName, IFormFile pdfFile)
        {
            try
            {
                string UploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "PDFs");
                string FilePath = Path.Combine(UploadFolder, pdfName);

                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(stream);
                };

                PDF pdf = new() 
                { 
                    Id = Id,
                    Name = pdfName,
                    FilePath = pdfName,
                };

                await _context.PDFs.AddAsync(pdf);
                return pdf;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdatePDFFile(string Id, IFormFile pdfFile)
        {
            try
            {
                var pdf = await GetPDF(Id);
                string UploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "PDFs");
                string FilePath = Path.Combine(UploadFolder, pdf.FilePath);
                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(stream);
                };
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdatePDF(PDF model)
        {
            try
            {
                await Task.FromResult(_context.PDFs.Update(model));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeletePDF(string Id)
        {
            try
            {
                var pdf = await _context.PDFs.FindAsync(Id) ?? throw new NullReferenceException("Database: PDF not found");

                string UploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "PDFs");
                string FilePath = Path.Combine(UploadFolder, pdf.FilePath);
                File.Delete(FilePath);

                await Task.FromResult(_context.PDFs.Remove(pdf));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
