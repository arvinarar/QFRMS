using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using QFRMS.Data.Interfaces;
using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StudentRepository> _logger;

        public StudentRepository(ApplicationDbContext context, ILogger<StudentRepository> logger)
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

        public async Task<IQueryable<Student>> RetrieveAllAsync()
        {
            try
            {
                return await Task.FromResult(_context.Set<Student>());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IQueryable<Grade>> RetrieveAllGradesAsync()
        {
            try
            {
                return await Task.FromResult(_context.Set<Grade>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<Student>> RetrieveStudentsFromBatchAsync(string Id)
        {
            try
            {
                return await Task.FromResult(_context.Set<Student>().Where(s => s.BatchId == Id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Student?> GetStudentAsync(string Id)
        {
            try
            {
                return await _context.Students.FindAsync(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Grade> GetStudentGradeAsync(string Id)
        {
            try
            {
                return await _context.Grades.FindAsync(Id) ?? throw new NullReferenceException("Grades not found");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CreateStudentAsync(Student model)
        {
            try
            {
                await _context.Students.AddAsync(model);
                await _context.Grades.AddAsync(new Grade { Student =  model, ULI = model.ULI });
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateStudentAsync(Student model)
        {
            try
            {
                await Task.FromResult(_context.Students.Update(model));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteStudentAsync(string Id)
        {
            try
            {
                var student = await _context.Students.FindAsync(Id) ?? throw new NullReferenceException();
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(NullReferenceException)
            {
                _logger.LogError("Database: Student not found");
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateGrades(IQueryable<Grade> grades)
        {
            try
            {
                _context.Grades.UpdateRange(grades);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateTrainingStatus(IQueryable<Student> students)
        {
            try
            {
                _context.Students.UpdateRange(students);
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
