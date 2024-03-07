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
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CourseRepository> _logger;

        public CourseRepository(ApplicationDbContext context, ILogger<CourseRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<Course>> RetrieveAllAsync()
        {
            try
            {
                return await Task.FromResult(_context.Set<Course>());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Course?> GetCourseAsync(string Id)
        {
            try
            {
                return await _context.Courses.FindAsync(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Course?> GetCourseOfBatchAsync(string Id)
        {
            try
            {
                return await Task.FromResult(_context.Set<Course>()
                    .Where(c => c.Id == Id)
                    .FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CreateCourseAsync(Course model)
        {
            try
            {
                await _context.Courses.AddAsync(model);
                await _context.SaveChangesAsync();
                return true;
            } 
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateCourseAsync(Course model)
        {
            try
            {
                var info = await GetCourseAsync(model.Id);
                if (info == null) return false;

                info.ProgramTitle = model.ProgramTitle;
                info.Sector = model.Sector;
                info.Status = model.Status;
                info.COPRNo = model.COPRNo;
                info.DeliveryMode = model.DeliveryMode;
                info.Duration = model.Duration;
                info.ClientClassification = model.ClientClassification;
                info.ScholarshipType = model.ScholarshipType;

                await Task.FromResult(_context.Courses.Update(info));
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteCourseAsync(string Id)
        {
            try
            {
                var course = await GetCourseAsync(Id);
                if(course == null) return false;

                await Task.FromResult(_context.Courses.Remove(course));
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
