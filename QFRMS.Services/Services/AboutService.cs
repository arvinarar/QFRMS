using Microsoft.Extensions.Logging;
using QFRMS.Data.Interfaces;
using QFRMS.Data.Models;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Services.Services
{
    public class AboutService : IAboutService
    {
        private readonly IAboutRepository _repository;
        private readonly ILogger<AboutService> _logger;
        private readonly Work _work = new();

        public AboutService(IAboutRepository repository, ILogger<AboutService> logger, Work work)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<InstituteInfo> GetInstituteInfoAsync()
        {
            try
            {
                return await _repository.GetInstituteInfosAsync() ?? throw new NullReferenceException("No Institute Info Found");
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime} GetInstituteInfoAsync Failed: {message}", DateTime.Now.ToString(), ex.Message);
                throw;
            }
        }

        public async Task<Work> UpdateInstituteInfoAsync(InstituteInfo model)
        {
            try
            {
                var update = await _repository.UpdateInstituteInfoAsync(model);
                if (!update) throw new Exception("Repository Problem");

                _work.Time = DateTime.Now;
                _work.Message = "Successfully Updated Institute Info";
                _work.Result = true;
                return _work;
            }
            catch (Exception ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = "Couldn't Update Institute Info";
                _work.Result = false;
                return _work;
            }
        }
    }
}
