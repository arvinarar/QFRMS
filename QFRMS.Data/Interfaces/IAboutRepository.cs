using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Interfaces
{
    public interface IAboutRepository
    {
        public Task<InstituteInfo?> GetInstituteInfosAsync();
        public Task<HomePageArticlesVideo?> GetHomePageArticlesVideo(string Id);
        public Task<IQueryable<HomePageArticlesVideo>> GetHomePageArticlesVideosAsync();
        public Task<bool> UpdateInstituteInfoAsync(InstituteInfo model);
        public Task<bool> UpdateHomePageArticlesVideoAsync(HomePageArticlesVideo model);
        public Task<bool> DeleteHomePageArticlesVideoAsync(string Id);
    }
}
