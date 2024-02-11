using Microsoft.Extensions.Logging;
using QFRMS.Data.Interfaces;
using QFRMS.Data.Models;
using QFRMS.Data.ViewModels;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Services.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;
        private readonly ILogger<CourseService> _logger;
        private readonly Work _work = new Work();

        public CourseService(ICourseRepository repository, ILogger<CourseService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IQueryable<CourseListViewModel>> GetCourseListAsync()
        {
            try
            {
                return await Task.FromResult(from course in await _repository.RetrieveAllAsync()
                                             orderby course.Sector, course.ProgramTitle
                                             select new CourseListViewModel
                                             {
                                                 Id = course.Id,
                                                 Sector = course.Sector,
                                                 ProgramTitle = course.ProgramTitle,
                                                 Status = course.Status,
                                                 COPRNo = course.COPRNo
                                             });
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime} GetCourseListAsync Failed: {message}", DateTime.Now.ToString(), ex.Message);
                throw;
            }
        }

        public async Task<CourseDetailViewModel> GetCourseDetailAsync(string Id)
        {
            try
            {
                var course = await _repository.GetCourseAsync(Id) ?? throw new NullReferenceException("Course not found");
                CourseDetailViewModel detail = new()
                {
                    Id = course.Id,
                    ProgramTitle= course.ProgramTitle,
                    Sector= course.Sector,
                    Status = course.Status,
                    COPRNo= course.COPRNo,
                    DeliveryMode = course.DeliveryMode,
                    Duration = course.Duration,
                    ClientClassification = course.ClientClassification,
                    ScholarshipType = course.ScholarshipType,
                    CanBeDeleted = false
                };
                //Check if can be deleted
                detail.CanBeDeleted = true;
                return detail;
            } 
            catch (Exception ex)
            {
                _logger.LogError("{datetime} GetCourseListAsync Failed: {message}", DateTime.Now.ToString(), ex.Message);
                throw;
            }
        }

        public async Task<Course> GetCourseEditViewAsync(string Id)
        {
            try
            {
                return await _repository.GetCourseAsync(Id) ?? throw new Exception("Course not Found");
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime} GetCourseEditViewAsync Failed: {message}", DateTime.Now.ToString(), ex.Message);
                throw;
            }
        }

        public async Task<IQueryable<CourseListViewModel>> SearchCourseListAsync(string searchType, string searchInput)
        {
            try
            {
                return searchType switch
                {
                    "Sector" => from course in await _repository.RetrieveAllAsync()
                                orderby course.Sector, course.ProgramTitle
                                where course.Sector.Contains(searchInput)
                                select new CourseListViewModel
                                {
                                    Id = course.Id,
                                    Sector = course.Sector,
                                    ProgramTitle = course.ProgramTitle,
                                    Status = course.Status,
                                    COPRNo = course.COPRNo
                                },

                    "Title" => from course in await _repository.RetrieveAllAsync()
                               orderby course.Sector, course.ProgramTitle
                               where course.ProgramTitle.Contains(searchInput)
                               select new CourseListViewModel
                               {
                                   Id = course.Id,
                                   Sector = course.Sector,
                                   ProgramTitle = course.ProgramTitle,
                                   Status = course.Status,
                                   COPRNo = course.COPRNo
                               },

                    "Status" => from course in await _repository.RetrieveAllAsync()
                                orderby course.Sector, course.ProgramTitle
                                where course.Status.Contains(searchInput)
                                select new CourseListViewModel
                                {
                                    Id = course.Id,
                                    Sector = course.Sector,
                                    ProgramTitle = course.ProgramTitle,
                                    Status = course.Status,
                                    COPRNo = course.COPRNo
                                },

                    "COPR" => from course in await _repository.RetrieveAllAsync()
                              orderby course.Sector, course.ProgramTitle
                              where course.COPRNo.Contains(searchInput)
                              select new CourseListViewModel
                              {
                                  Id = course.Id,
                                  Sector = course.Sector,
                                  ProgramTitle = course.ProgramTitle,
                                  Status = course.Status,
                                  COPRNo = course.COPRNo
                              },

                    _ => from course in await _repository.RetrieveAllAsync()
                         orderby course.Sector, course.ProgramTitle
                         select new CourseListViewModel
                         {
                             Id = course.Id,
                             Sector = course.Sector,
                             ProgramTitle = course.ProgramTitle,
                             Status = course.Status,
                             COPRNo = course.COPRNo
                         }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime} SearchCourseListAsync Failed: {message}", DateTime.Now.ToString(), ex.Message);
                throw;
            }
        }

        public async Task<Work> AddCourseAsync(Course model)
        {
            try
            {
                model.Id = Guid.NewGuid().ToString();
                var work = await _repository.CreateCourseAsync(model);
                if (!work) throw new Exception("Work failed");

                _work.Time = DateTime.Now;
                _work.Message = "Successfully Created Course";
                _work.Result = true;
                return _work;
            }
            catch(Exception ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = "Couldn't Create course";
                _work.Result = false;
                return _work;
            }
        }

        public async Task<Work> EditCourseAsync(Course model)
        {
            try
            {
                var work = await _repository.UpdateCourseAsync(model);
                if (!work) throw new Exception("Work failed");

                _work.Time = DateTime.Now;
                _work.Message = "Successfully Updated Course";
                _work.Result = true;
                return _work;
            }
            catch (Exception ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = "Couldn't Update course";
                _work.Result = false;
                return _work;
            }
        }

        public async Task<Work> DeleteCourseAsync(string Id)
        {
            try
            {
                //Check if it still has batches
                var work = await _repository.DeleteCourseAsync(Id);
                if (!work) throw new Exception("Work failed");

                _work.Time = DateTime.Now;
                _work.Message = "Successfully Deleted Course";
                _work.Result = true;
                return _work;
            }
            catch (Exception ex) 
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = "Couldn't Delete course";
                _work.Result = false;
                return _work;
            }
        }
    }
}
