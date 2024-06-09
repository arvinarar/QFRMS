using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QFRMS.Data.DTOs;
using QFRMS.Data.Interfaces;
using QFRMS.Data.Models;
using QFRMS.Data.ViewModels;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Utils;
using System;

namespace QFRMS.Services.Services
{
    public class MemoService : IMemoService
    {
        private readonly IMemoRepository _repository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IPDFRepository _pdfRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<MemoService> _logger;
        private readonly Work _work = new Work();

        public MemoService(IMemoRepository repository, IUserAccountRepository userAccountRepository, IPDFRepository pdfRepository, IWebHostEnvironment webHostEnvironment, ILogger<MemoService> logger)
        {
            _repository = repository;
            _userAccountRepository = userAccountRepository;
            _pdfRepository = pdfRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<Memo> GetMemoAsync(int? id)
        {
            try
            {
                return await _repository.RetrieveMemoAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<MemoListViewModel>> GetMemoList()
        {
            return await Task.FromResult((from memo in await _repository.RetrieveAllAsync()
                                         join pdf in await _pdfRepository.RetrieveAllAsync() on memo.FileId equals pdf.Id
                                         orderby memo.DateUploaded descending
                                         select new MemoListViewModel
                                         {
                                             Id = memo.Id,
                                             DateUploaded = memo.DateUploaded!.Value.ToString("MM/dd/yyyy - hh:mm tt"),
                                             Name = pdf.Name
                                         }).Skip(1));
        }

        public async Task<Work> UploadMemoAsync(UploadMemo model)
        {
            try
            {
                string UploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "PDFs");
                string DateUploaded = DateTime.Now.ToString("yyyy-mm-dd.H-mm");
                string pdfName = DateUploaded + "Memo.pdf";
                string FilePath = Path.Combine(UploadFolder, pdfName);

                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }

                PDF memo = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.File.FileName,
                    FilePath = pdfName
                };

                var work = await _repository.UploadMemo(memo);

                _work.Time = DateTime.Now;
                _work.Message = "Successfully uploaded Memo.";
                _work.Result = true;
                return _work;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FileContentResult> DownloadMemo(int id)
        {
            try
            {
                var pdf = await _repository.GetMemo(id);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "PDFs");
                string pdfPath = Path.Combine(path, pdf.FilePath);
                if(!File.Exists(pdfPath)) throw new NullReferenceException("File Not Found");
                return new FileContentResult(File.ReadAllBytes(pdfPath), "application/pdf");
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<bool> HasSeenMemo(string name)
        {
            try
            {
                var user = await _userAccountRepository.GetUserByName(name) ?? throw new NullReferenceException("User doesn't exist");
                var HasSeenMemo = await _repository.HasSeenMemo(user.Id);
                return HasSeenMemo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Work> DeleteMemoAsync(int id)
        {
            try
            {
                var memo = await _repository.RetrieveMemoAsync(id) ?? throw new NullReferenceException("Database: memo not found.");
                var pdf = await _pdfRepository.GetPDF(memo.FileId!);
                await _pdfRepository.DeletePDF(memo.FileId!);
                var work = await _repository.DeleteMemo(id);

                _work.Time = DateTime.Now;
                _work.Message = $"Successfully deleted memo \'{pdf.Name}\'.";
                _work.Result = true;
                return _work;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
