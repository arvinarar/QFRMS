using QFRMS.Data.Models;
using QFRMS.Data.DTOs;
using QFRMS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Services.Interfaces
{
    public interface IAboutService
    {
        public Task<InstituteInfo> GetInstituteInfoAsync();
        public Task<UpdateArticleVideo> GetUpdateArticleVideo(string Id);
        public Task<List<HomePageArticlesVideo>> GetHomePageArticlesVideosAsync();
        public Task<Work> UpdateInstituteInfoAsync(InstituteInfo model);
        public Task<Work> UpdateHomePageArticlesVideoAsync(UpdateArticleVideo model);
        public Task<Work> DeleteHomePageArticlesVideoAsync(string Id);
    }
}
