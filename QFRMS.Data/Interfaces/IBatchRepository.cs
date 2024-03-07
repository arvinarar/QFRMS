using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Interfaces
{
    public interface IBatchRepository
    {
        public Task<int> SaveChangesAsync();
        public Task<IQueryable<Batch>> RetrieveAllAsync();
        public Task<Batch?> GetBatchAsync(string Id);
        public Task<DeploymentDetails> GetDeploymentDetailsAsync(string Id);
        public Task<IQueryable<Batch>> GetBatchesFromCourse(string Id);
        public Task<bool> CreateBatchAsync(Batch batch, DeploymentDetails deploymentDetails);
        public Task<bool> UpdateBatchAsync(Batch batch, DeploymentDetails deploymentDetails);
        public Task<bool> DeleteBatchAsync(string Id);
    }
}
