using Microsoft.AspNetCore.Identity;
using QFRMS.Data.DTOs;
using QFRMS.Data.Models;
using QFRMS.Data.Repositories;
using QFRMS.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Interfaces
{
    public interface IUserAccountRepository
    {
        public Task<IQueryable<UserAccount>> GetUsersAsync();
        public Task<IQueryable<IdentityRole>> GetRolesAsync();
        public Task<IQueryable<IdentityUserRole<string>>> GetUserRolesAsync();
        public Task<UserAccount?> GetUserByIdAsync(string Id);
        public Task<int> SaveChangesAsync();
    }
}
