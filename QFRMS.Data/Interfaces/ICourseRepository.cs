using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Interfaces
{
    public interface ICourseRepository
    {
        public Task<int> SaveChangesAsync();
        public Task<IQueryable<Course>> RetrieveAllAsync();
        public Task<Course?> GetCourseAsync(string Id);
        public Task<bool> CreateCourseAsync(Course model);
        public Task<bool> UpdateCourseAsync(Course model);
        public Task<bool> DeleteCourseAsync(string Id);
    }
}
