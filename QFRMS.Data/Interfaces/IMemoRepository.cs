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
        public Task<Memo> RetrieveMemoAsync();
        public Task<bool> UploadMemo(PDF file);
        public Task<PDF> GetMemo();
        public Task<bool> HasSeenMemo(string Id);
    }
}
