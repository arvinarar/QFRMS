using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFRMS.Data.ViewModels;
using QFRMS.Data.Models;
using QFRMS.Data.DTOs;
using QFRMS.Services.Utils;

namespace QFRMS.Services.Interfaces
{
    public interface IUserAccountService
    {
        public Task<IQueryable<UsersViewModel>> GetAllUsersAsync();
        public Task<UsersViewModel> GetUserById(string Id);
        public Task<UpdateUserDetails> GetUserDetails(string Id);
        public Task<IQueryable<UsersViewModel>> SearchUsersAsync(string searchType, string searchInput);
        public Task<UpdateUser?> GetUpdateUserDTO(string Id);
        public Task<Work> CreateUser(Register model);
        public Task<Work> UpdateUser(UpdateUser model);
        public Task<Work> UpdateUserDetails(UpdateUserDetails model);
        public Task<Work> DeleteUser(string Id);
    }
}
