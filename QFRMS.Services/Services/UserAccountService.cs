using QFRMS.Services.Interfaces;
using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFRMS.Data.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using QFRMS.Data.ViewModels;
using QFRMS.Data.DTOs;
using static QFRMS.Data.Enums.Enums;
using static QFRMS.Data.Enums.EnumHelper;
using static QFRMS.Data.Constants;
using QFRMS.Services.Utils;
using System.Data;
using QFRMS.Data;
using QFRMS.Data.Enums;

namespace QFRMS.Services.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _repository;
        private readonly ILogger<UserAccountService> _logger;
        private readonly UserManager<UserAccount> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Work _work = new();

        public UserAccountService(IUserAccountRepository repository, ILogger<UserAccountService> logger, UserManager<UserAccount>  userManager, RoleManager<IdentityRole> roleManager)
        {
            _repository = repository;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IQueryable<UsersViewModel>> GetAllUsersAsync()
        {
            try
            {
                return await Task.FromResult(from users in await _repository.GetUsersAsync()
                                             join urole in await _repository.GetUserRolesAsync() on users.Id equals urole.UserId
                                             join role in await _repository.GetRolesAsync() on urole.RoleId equals role.Id
                                             orderby users.FirstName
                                             select new UsersViewModel
                                             {
                                                 Id = users.Id,
                                                 FullName = $"{users.FirstName} {users.MiddleName} {users.LastName} {users.ExtensionName}",
                                                 UserName = users.UserName,
                                                 Role = role.Name
                                             });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UsersViewModel> GetUserById(string Id)
        {
            try
            {
                var user = await _repository.GetUserByIdAsync(Id) ?? throw new NullReferenceException("User not Found");
                var getRole = _userManager.GetRolesAsync(user!).Result.FirstOrDefault() ?? throw new NullReferenceException("Couldn't Retrieve Role");
                return new UsersViewModel
                {
                    Id = user.Id,
                    FullName = $"{user.FirstName} {user.MiddleName} {user.LastName} {user.ExtensionName}",
                    UserName = user.UserName,
                    Role = getRole
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UpdateUserDetails> GetUserDetails(string Id)
        {
            try
            {
                var user = await _repository.GetUserByIdAsync(Id) ?? throw new NullReferenceException("User not Found.");
                var getRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault() ?? throw new NullReferenceException("Couldn't Retrieve Role.");
                return new UpdateUserDetails
                {
                    Id = user.Id,
                    FullName = $"{user.FirstName} {user.MiddleName} {user.LastName} {user.ExtensionName}",
                    Username = user.UserName ?? throw new NullReferenceException("UserName not Found."),
                    Role = getRole
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<UsersViewModel>> SearchUsersAsync(string searchType, string searchInput)
        {
            try
            {
                return searchType switch
                {
                    "Name" => from users in await _repository.GetUsersAsync()
                              join urole in await _repository.GetUserRolesAsync() on users.Id equals urole.UserId
                              join role in await _repository.GetRolesAsync() on urole.RoleId equals role.Id
                              orderby users.FirstName
                              where
                                  users.FirstName!.Contains(searchInput) ||
                                  users.MiddleName.Contains(searchInput) ||
                                  users.LastName!.Contains(searchInput) ||
                                  users.ExtensionName!.Contains(searchInput)
                              select new UsersViewModel
                              {
                                  Id = users.Id,
                                  FullName = $"{users.FirstName} {users.MiddleName} {users.LastName} {users.ExtensionName}",
                                  UserName = users.UserName,
                                  Role = role.Name
                              },

                    "UserName" => from users in await _repository.GetUsersAsync()
                                  join urole in await _repository.GetUserRolesAsync() on users.Id equals urole.UserId
                                  join role in await _repository.GetRolesAsync() on urole.RoleId equals role.Id
                                  orderby users.FirstName
                                  where
                                       users.UserName!.Contains(searchInput)
                                  select new UsersViewModel
                                  {
                                      Id = users.Id,
                                      FullName = $"{users.FirstName} {users.MiddleName} {users.LastName} {users.ExtensionName}",
                                      UserName = users.UserName,
                                      Role = role.Name
                                  },

                    "Role" => from users in await _repository.GetUsersAsync()
                              join urole in await _repository.GetUserRolesAsync() on users.Id equals urole.UserId
                              join role in await _repository.GetRolesAsync() on urole.RoleId equals role.Id
                              orderby users.FirstName
                              where
                                   role.Name!.Contains(searchInput)
                              select new UsersViewModel
                              {
                                  Id = users.Id,
                                  FullName = $"{users.FirstName} {users.MiddleName} {users.LastName} {users.ExtensionName}",
                                  UserName = users.UserName,
                                  Role = role.Name
                              },

                    _ => from users in await _repository.GetUsersAsync()
                         join urole in await _repository.GetUserRolesAsync() on users.Id equals urole.UserId
                         join role in await _repository.GetRolesAsync() on urole.RoleId equals role.Id
                         orderby users.FirstName
                         select new UsersViewModel
                         {
                             Id = users.Id,
                             FullName = $"{users.FirstName} {users.MiddleName} {users.LastName} {users.ExtensionName}",
                             UserName = users.UserName,
                             Role = role.Name
                         },
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UpdateUser?> GetUpdateUserDTO(string Id)
        {
            try
            {
                var user = await _repository.GetUserByIdAsync(Id) ?? throw new NullReferenceException("User not Found.");
                var getRole = _userManager.GetRolesAsync(user!).Result.FirstOrDefault() ?? throw new NullReferenceException("Couldn't Retrieve Role.");
                UserRoles role = Enum.Parse<UserRoles>(getRole!, true);

                return new UpdateUser
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    ExtensionName = user.ExtensionName,
                    Username = user.UserName,
                    Role = role,
                    ResetPassword = false,
                    NewPassword = ""
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Work> CreateUser(Register model)
        {
            try
            {
                if (await _userManager.FindByNameAsync(model.Username!) != null)
                    throw new ArgumentException("User already exist.");
                UserAccount user = new()
                {
                    UserName = model.Username,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    ExtensionName = model.ExtensionName,
                };
                var role = _roleManager.FindByNameAsync(GetEnumDescription(model.userRoles)).Result
                    ?? throw new NullReferenceException("Role not Found.");
                var createUser = await _userManager.CreateAsync(user, model.Password!);
                _ = await _userManager.AddToRoleAsync(user, role.Name!);

                _work.Time = DateTime.Now;
                _work.Message = "Successfully created user.";
                _work.Result = true;
                return _work;
            }
            catch (ArgumentException ex)
            {
                _work.ErrorCode = ErrorType.Argument;
                _work.Time = DateTime.Now;
                _work.Message = ex.Message;
                _work.Result = false;
                return _work;
            }
            catch (NullReferenceException ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = ex.Message;
                _work.Result = false;
                return _work;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Work> UpdateUser(UpdateUser model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id) ?? throw new NullReferenceException("Couldn't retrieve user.");
                var oldRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault() ?? throw new NullReferenceException("Couldn't retrieve role.");

                user.FirstName = model.FirstName;
                user.MiddleName = model.MiddleName;
                user.LastName = model.LastName;
                user.ExtensionName = model.ExtensionName;
                user.UserName = model.Username;

                if (model.ResetPassword)
                {
                    if (model.NewPassword == null) throw new ArgumentException("New Password Field is null.");
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                }
                _ = await _userManager.UpdateAsync(user);
                _ = await _userManager.RemoveFromRoleAsync(user, oldRole);
                _ = await _userManager.AddToRoleAsync(user, model.Role.ToString());

                await _repository.SaveChangesAsync();
                
                _work.Time = DateTime.Now;
                _work.Message = "Successfully updated user.";
                _work.Result = true;
                return _work;
            }
            catch (ArgumentException ex)
            {
                _work.ErrorCode = ErrorType.Argument;
                _work.Time = DateTime.Now;
                _work.Message = ex.Message;
                _work.Result = false;
                return _work;
            }
            catch (NullReferenceException ex)
            {
                _work.ErrorCode = ErrorType.Generic;
                _work.Time = DateTime.Now;
                _work.Message = ex.Message;
                _work.Result = false;
                return _work;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Work> UpdateUserDetails(UpdateUserDetails model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id) ?? throw new NullReferenceException("Couldn't retrieve user.");
                user.UserName = model.Username;

                if (model.OldPassword != null)
                {
                    if (model.NewPassword == null) throw new ArgumentException("New Password must not be empty.");
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if(!result.Succeeded) throw new ArgumentException("Old and New Password does not match.");
                }

                _ = await _userManager.UpdateAsync(user);
                await _repository.SaveChangesAsync();

                _work.Time = DateTime.Now;
                _work.Message = "Successfully updated user details.";
                _work.Result = true;
                return _work;
            }
            catch (ArgumentException ex)
            {
                _work.ErrorCode = ErrorType.Argument;
                _work.Time = DateTime.Now;
                _work.Message = ex.Message;
                _work.Result = false;
                return _work;
            }
            catch (NullReferenceException ex)
            {
                _work.ErrorCode = ErrorType.Generic;
                _work.Time = DateTime.Now;
                _work.Message = ex.Message;
                _work.Result = false;
                return _work;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Work> DeleteUser(string Id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Id!) ?? throw new NullReferenceException("Couldn't retrieve user.");
                var result = await _userManager.DeleteAsync(user);
                if(result.Succeeded)
                {
                    _work.Time = DateTime.Now;
                    _work.Message = "Successfully deleted user.";
                    _work.Result = true;
                    return _work;
                }
                else
                {
                    throw new Exception(result.Errors.ToString());
                }
            }
            catch (NullReferenceException ex)
            {
                _work.ErrorCode = ex.Message;
                _work.Time = DateTime.Now;
                _work.Message = ErrorType.Generic;
                _work.Result = false;
                return _work;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
