using Microsoft.AspNetCore.Mvc;
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
    public interface IBatchService
    {
        public Task<IQueryable<BatchListViewModel>> GetBatchListAsync();
        public Task<IQueryable<BatchListViewModel>> SearchBatchListAsync(string searchType, string searchInput);
        public Task<IQueryable<BatchCourseListViewModel>> SearchBatchCourseListAsync(string courseId, string searchType, string searchInput);
        public Task<CreateBatch> GetCreateBatchDTOAsync(string? courseId);
        public Task<UpdateBatch> GetUpdateBatchDTOAsync(string Id, bool FromCoursePage = false);
        public Task<BatchDetailViewModel> GetBatchDetailAsync(string Id, bool FromCoursePage = false);
        public Task<Work> AddBatchAsync(CreateBatch model);
        public Task<Work> UpdateBatchAsync(UpdateBatch model);
        public Task<Work> DeleteBatchAsync(string Id);
        public Task<FileContentResult> GetDocument(string Id);
    }
}
