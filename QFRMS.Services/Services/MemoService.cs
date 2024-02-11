using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QFRMS.Data.DTOs;
using QFRMS.Data.Interfaces;
using QFRMS.Data.Models;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Utils;

namespace QFRMS.Services.Services
{
    public class MemoService : IMemoService
    {
        private readonly IMemoRepository _repository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly ILogger<MemoService> _logger;
        private readonly Work _work = new Work();

        public MemoService(IMemoRepository repository, IUserAccountRepository userAccountRepository, ILogger<MemoService> logger)
        {
            _repository = repository;
            _userAccountRepository = userAccountRepository;
            _logger = logger;
        }

        public async Task<Memo> GetMemoAsync()
        {
            try
            {
                return await _repository.RetrieveMemoAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime} GetMemoAsync Failed: {message}", DateTime.Now.ToString(), ex.Message);
                throw;
            }
        }

        public async Task<Work> UploadMemoAsync(UploadMemo model)
        {
            try
            {
                PDF memo = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.File.FileName,
                    File = []
                };

                using(var stream = new MemoryStream())
                {
                    model.File.CopyTo(stream);
                    memo.File = stream.ToArray();
                }

                var work = await _repository.UploadMemo(memo);
                if(!work) throw new Exception("Work failed");

                _work.Time = DateTime.Now;
                _work.Message = "Successfully Uploaded Memo";
                _work.Result = true;
                return _work;
            }
            catch (Exception ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = "Couldn't Upload Memo";
                _work.Result = false;
                return _work;
            }
        }

        public async Task<FileContentResult> DownloadMemo()
        {
            try
            {
                var pdf = await _repository.GetMemo();
                return new FileContentResult(pdf.File, "application/pdf");
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
                var user = await _userAccountRepository.GetUserByName(name) ?? throw new Exception("User doesn't exist");
                var HasSeenMemo = await _repository.HasSeenMemo(user.Id);
                return HasSeenMemo;
            }
            catch(Exception ex)
            {
                _logger.LogError("{datetime} HasSeenMemo Failed: {message}", DateTime.Now.ToString(), ex.Message);
                return true;
            }
        }
    }
}
