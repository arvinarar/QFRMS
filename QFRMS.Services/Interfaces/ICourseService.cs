using QFRMS.Data.Models;
using QFRMS.Data.ViewModels;
using QFRMS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Services.Interfaces
{
    public interface ICourseService
    {
        public Task<IQueryable<CourseListViewModel>> GetCourseListAsync();
        public Task<CourseDetailViewModel> GetCourseDetailAsync(string Id);
        public Task<Course> GetCourseEditViewAsync(string Id);
        public Task<IQueryable<CourseListViewModel>> SearchCourseListAsync(string searchType, string searchInput);
        public Task<Work> AddCourseAsync(Course model);
        public Task<Work> EditCourseAsync(Course model);
        public Task<Work> DeleteCourseAsync(string Id);
    }
}
