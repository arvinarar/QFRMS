using Microsoft.AspNetCore.Identity;
using QFRMS.Data.ViewModels;
using QFRMS.Data.Interfaces;
using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace QFRMS.Data.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserAccountRepository> _logger;
        private readonly UserManager<UserAccount> _userManager;

        public UserAccountRepository(ApplicationDbContext context, UserManager<UserAccount> userManager, ILogger<UserAccountRepository> logger)
        {
            _context = context;
            _userManager = userManager;
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

        public async Task<IQueryable<UserAccount>> GetUsersAsync()
        {
            try
            {
                return await Task.FromResult(_context.Set<UserAccount>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<IdentityRole>> GetRolesAsync()
        {
            try
            {
                return await Task.FromResult(_context.Set<IdentityRole>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<IdentityUserRole<string>>> GetUserRolesAsync()
        {
            try
            {
                return await Task.FromResult(_context.Set<IdentityUserRole<string>>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserAccount?> GetUserByIdAsync(string Id)
        {
            try
            {
                return await _context.Users.FindAsync(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}
