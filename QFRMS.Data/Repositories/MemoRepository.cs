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
    public class MemoRepository : IMemoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MemoRepository> _logger;

        public MemoRepository(ApplicationDbContext context, ILogger<MemoRepository> logger)
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

        public async Task<IQueryable<Memo>> RetrieveAllAsync()
        {
            try
            {
                return await Task.FromResult(_context.Set<Memo>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Memo> RetrieveMemoAsync(int? id)
        {
            try
            {
                if(id == null)
                {
                    var data = _context.Memo.OrderByDescending(m => m.DateUploaded).FirstOrDefault() ?? throw new Exception("Database: Memo not found");
                    data.File = await _context.PDFs.FindAsync(data.FileId);
                    return data;
                }
                else
                {
                    var data = _context.Memo.Find(id) ?? throw new Exception("Database: Memo not found");
                    data.File = await _context.PDFs.FindAsync(data.FileId);
                    return data;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UploadMemo(PDF file)
        {
            try
            {
                var memo = new Memo 
                { 
                    DateUploaded = DateTime.Now, 
                    FileId = file.Id, 
                    File = file 
                };

                await _context.PDFs.AddAsync(file);
                _context.Memo.Add(memo);

                //Clear SeenUsers Table
                var toDelete = _context.SeenUsers.Select(a => new SeenUsers { UserId = a.UserId }).ToList();
                _context.SeenUsers.RemoveRange(toDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteMemo(int id)
        {
            try
            {
                var memo = _context.Memo.Find(id) ?? throw new Exception("Database: Memo not found");
                _context.Memo.Remove(memo);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PDF> GetMemo(int id)
        {
            try
            {
                var memo = await _context.Memo.FindAsync(id) ?? throw new Exception("Database: Memo not found");
                return await _context.PDFs.FindAsync(memo.FileId) ?? throw new Exception("No Memo File Found");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> HasSeenMemo(string Id)
        {
            try
            {
                var userId = await _context.SeenUsers.FindAsync(Id);
                if (userId != null) 
                    return true;
                else
                {
                    await _context.SeenUsers.AddAsync(new SeenUsers { UserId = Id });
                    await _context.SaveChangesAsync();
                    return false;
                }   
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
