using QFRMS.Data.DTOs;
using QFRMS.Data.ViewModels;
using QFRMS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Services.Interfaces
{
    public interface IStudentService
    {
        public Task<IQueryable<StudentListViewModel>> GetStudentListAsync(string? TrainorName);
        public Task<IQueryable<StudentListViewModel>> SearchStudentListAsync(string searchType, string searchInput, string? TrainorName);
        public Task<IQueryable<StudentListViewModel>> SearchStudentBatchListAsync(string batchId, string searchType, string searchInput);
        public Task<EnrollStudent> CreateStudentDTOAsync(string? batchId, bool FromCoursePage = false);
        public Task<EditStudent> EditStudentDTOAsync(string ULI, string? batchId, bool FromCoursePage = false);
        public Task<Work> EnrollStudentAsync(EnrollStudent model);
        public Task<Work> EditStudentAsync(EditStudent model);
        public Task<Work> DeleteStudentAsync(string ULI);
        public Task<Work> UpdateGrades(StudentGradesList model);
        public Task<bool> CheckIfAlreadyExist(string ULI);
        public Task<StudentDetailViewModel> GetStudentDetail(string ULI, string? BatchId, bool fromCoursePage);
        public Task<StudentGradesList> GetStudentGrades(string BatchId, bool isTrainor, bool fromCoursePage);
    }
}
