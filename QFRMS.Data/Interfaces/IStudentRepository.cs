using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Interfaces
{
    public interface IStudentRepository
    {
        public Task<int> SaveChangesAsync();
        public Task<IQueryable<Student>> RetrieveAllAsync();
        public Task<IQueryable<Grade>> RetrieveAllGradesAsync();
        public Task<IQueryable<Student>> RetrieveStudentsFromBatchAsync(string Id);
        public Task<Student?> GetStudentAsync(string Id);
        public Task<Grade> GetStudentGradeAsync(string Id);
        public Task<bool> CreateStudentAsync(Student model);
        public Task<bool> UpdateStudentAsync(Student model);
        public Task<bool> DeleteStudentAsync(string Id);
        public Task<bool> UpdateGrades(IQueryable<Grade> grades);
        public Task<bool> UpdateTrainingStatus(IQueryable<Student> students);
    }
}
