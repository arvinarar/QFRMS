using Microsoft.EntityFrameworkCore;
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
    public class BatchRepository : IBatchRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BatchRepository> _logger;

        public BatchRepository(ApplicationDbContext context, ILogger<BatchRepository> logger)
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

        public async Task<IQueryable<Batch>> RetrieveAllAsync()
        {
            try
            {
                return await Task.FromResult(_context.Set<Batch>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Batch?> GetBatchAsync(string Id)
        {
            try
            {
                var batch = await _context.Batches.FindAsync(Id);
                if(batch != null)
                    batch.DeploymentDetails = await _context.DeploymentDetails.FirstOrDefaultAsync(d => d.BatchId == batch.Id);
                return batch;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DeploymentDetails> GetDeploymentDetailsAsync(string Id)
        {
            try
            {
                var deploymentDetails = await _context.DeploymentDetails.FindAsync(Id) ?? throw new NullReferenceException("Database: Deployment Details not Found");
                return deploymentDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<Batch>> GetBatchesFromCourse(string Id)
        {
            try
            {
                return await Task.FromResult(_context.Set<Batch>().Where(b => b.CourseId == Id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CreateBatchAsync(Batch batch, DeploymentDetails deploymentDetails)
        {
            try
            {
                await _context.Batches.AddAsync(batch);
                await _context.DeploymentDetails.AddAsync(deploymentDetails);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateBatchAsync(Batch batch, DeploymentDetails deploymentDetails)
        {
            try
            {
                await Task.FromResult(_context.Batches.Update(batch));
                await Task.FromResult(_context.DeploymentDetails.Update(deploymentDetails));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteBatchAsync(string Id)
        {
            try
            {
                var batch = await GetBatchAsync(Id);
                if (batch == null) return false;

                await Task.FromResult(_context.Batches.Remove(batch));
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
