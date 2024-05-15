using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Interfaces
{
    public interface IMemoRepository
    {
        public Task<int> SaveChangesAsync();
        public Task<IQueryable<Memo>> RetrieveAllAsync();
        public Task<Memo> RetrieveMemoAsync(int? id);
        public Task<bool> UploadMemo(PDF file);
        public Task<bool> DeleteMemo(int id);
        public Task<PDF> GetMemo(int id);
        public Task<bool> HasSeenMemo(string Id);
    }
}
