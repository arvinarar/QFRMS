using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using QFRMS.Data.DTOs;
using QFRMS.Data.Interfaces;
using QFRMS.Data.Models;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Services.Services
{
    public class AboutService : IAboutService
    {
        private readonly IAboutRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<AboutService> _logger;
        private readonly Work _work = new();

        public AboutService(IAboutRepository repository, IWebHostEnvironment webHostEnvironment, ILogger<AboutService> logger, Work work)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<InstituteInfo> GetInstituteInfoAsync()
        {
            try
            {
                return await _repository.GetInstituteInfosAsync() ?? throw new NullReferenceException("No Institute Info Found");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UpdateArticleVideo> GetUpdateArticleVideo(string Id)
        {
            try
            {
                var result = await _repository.GetHomePageArticlesVideo(Id) ?? throw new NullReferenceException("No Institute Info Found");
                return new UpdateArticleVideo 
                {
                    Id = Id,
                    Title = result.Title,
                    Description = result.Description
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<HomePageArticlesVideo>> GetHomePageArticlesVideosAsync()
        {
            try
            {
                var result = await _repository.GetHomePageArticlesVideosAsync();
                return [.. result];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Work> UpdateInstituteInfoAsync(InstituteInfo model)
        {
            try
            {
                var update = await _repository.UpdateInstituteInfoAsync(model);
                if (!update) throw new Exception("Repository Problem.");

                _work.Time = DateTime.Now;
                _work.Message = "Successfully updated institute info.";
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

        public async Task<Work> UpdateHomePageArticlesVideoAsync(UpdateArticleVideo model)
        {
            try
            {
                var data = await _repository.GetHomePageArticlesVideo(model.Id) ?? throw new NullReferenceException("UpdateArticleVideo not found.");
                data.Title = model.Title;
                data.Description = model.Description;
                if(model.File != null)
                {
                    //Delete Old File if exist
                    string Folder = Path.Combine(_webHostEnvironment.WebRootPath, "homepage");
                    if(data.FilePath != null)
                    {
                        string DeleteFilePath = Path.Combine(Folder, data.FilePath);
                        if (File.Exists(DeleteFilePath))
                            File.Delete(DeleteFilePath);
                    }

                    string FileName = $"{model.Id}_{DateTime.Now:HH-mm-ss}_{model.File.FileName}";
                    string FilePath = Path.Combine(Folder, FileName);

                    using (var stream = new FileStream(FilePath, FileMode.Create))
                    {
                        await model.File.CopyToAsync(stream);
                    }
                    data.FilePath = FileName;
                }
                var work = await _repository.UpdateHomePageArticlesVideoAsync(data);

                var name = model.Id.Equals("1") ? "Home Video" : $"Home Article {int.Parse(model.Id) - 1}";
                _work.Time = DateTime.Now;
                _work.Message = $"Successfully Updated {name}";
                _work.Result = true;
                return _work;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Work> DeleteHomePageArticlesVideoAsync(string Id)
        {
            try
            {
                var data = await _repository.GetHomePageArticlesVideo(Id) ?? throw new NullReferenceException("UpdateArticleVideo not found.");

                //Delete File First
                string Folder = Path.Combine(_webHostEnvironment.WebRootPath, "homepage");
                string FilePath = Path.Combine(Folder, data.FilePath ?? throw new NullReferenceException("ArticleVideo File Path not found."));
                File.Delete(FilePath);

                await _repository.DeleteHomePageArticlesVideoAsync(Id);
                var name = Id.Equals("1") ? "Home Video" : $"Home Article {int.Parse(Id) - 1}";
                _work.Time = DateTime.Now;
                _work.Message = $"Successfully Deleted {name}.";
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
