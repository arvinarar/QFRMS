using Microsoft.AspNetCore.Mvc;
using QFRMS.Data.DTOs;
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
    public interface IMemoService
    {
        public Task<Memo> GetMemoAsync(int? id);
        public Task<IQueryable<MemoListViewModel>> GetMemoList();
        public Task<Work> UploadMemoAsync(UploadMemo model);
        public Task<FileContentResult> DownloadMemo(int id);
        public Task<bool> HasSeenMemo(string name);
        public Task<Work> DeleteMemoAsync(int id);
    }
}
