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
            return await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<UserAccount>> GetUsersAsync()
        {
            return await Task.FromResult(_context.Set<UserAccount>());
        }

        public async Task<IQueryable<IdentityRole>> GetRolesAsync()
        {
            return await Task.FromResult(_context.Set<IdentityRole>());
        }

        public async Task<IQueryable<IdentityUserRole<string>>> GetUserRolesAsync()
        {
            return await Task.FromResult(_context.Set<IdentityUserRole<string>>());
        }

        public async Task<UserAccount?> GetUserByIdAsync(string Id)
        {
            return await _context.Users.FindAsync(Id);
        }

        
    }
}
