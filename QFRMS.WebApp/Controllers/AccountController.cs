using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QFRMS.Data.Models;
using QFRMS.Data.DTOs;
using QFRMS.Data.ViewModels;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Utils;
using static QFRMS.Data.Constants;
using static QFRMS.Data.Helper;

namespace QFRMS.WebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly UserManager<UserAccount> _userManager;
        private readonly IFileLogger _fileLogger;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserAccountService _userAccountService;
        private readonly int _pageSize = 10;
        public AccountController(
            SignInManager<UserAccount> signInManager, 
            UserManager<UserAccount> userManager,
            IFileLogger fileLogger, 
            ILogger<AccountController> logger,
            IUserAccountService userAccountService)
        {
            _signInManager = signInManager;
            _fileLogger = fileLogger;
            _logger = logger;
            _userAccountService = userAccountService;
            _userManager = userManager;
        }


        /// <summary>
        /// Shows a paginated table containing all users
        /// </summary>
        /// <param name="pageNumber">Page number of the paginated list</param>
        /// <returns>A paginated table of users</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            try
            {
                var data = await _userAccountService.GetAllUsersAsync();
                return View(await PaginatedList<UsersViewModel>.CreateAsync(data, pageNumber ?? 1, _pageSize));
            } catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to retrieve users. Error: {Message}", DateTime.Now.ToString(), ex.Message);
                var dummyList = new List<UsersViewModel>();
                return View(await PaginatedList<UsersViewModel>.CreateAsync(dummyList, pageNumber ?? 1, _pageSize));
            }
        }

        /// <summary>
        /// Filter User/s based on the search parameter and input
        /// </summary>
        /// <param name="searchType">Narrows down the search</param>
        /// <param name="searchInput">the search input field value</param>
        /// <param name="pageNumber">Page number of the paginated list</param>
        /// <returns>The Filtered table based on the search result</returns>
        // GET : Search
        public PartialViewResult Search(string searchType, string searchInput, int? pageNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(searchInput))
                {
                    var result = _userAccountService.GetAllUsersAsync().Result;
                    return PartialView("_UserList", PaginatedList<UsersViewModel>.CreateAsync(result, pageNumber ?? 1, _pageSize).Result);
                }
                else
                {
                    var result = _userAccountService.SearchUsersAsync(searchType, searchInput).Result;
                    return PartialView("_UserList", PaginatedList<UsersViewModel>.CreateAsync(result, pageNumber ?? 1, _pageSize).Result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Searched Failed. Error: {Message}", DateTime.Now.ToString(), ex.Message);
                var dummyList = new List<UsersViewModel>();
                return PartialView("_UserList", PaginatedList<UsersViewModel>.CreateAsync(dummyList, pageNumber ?? 1, _pageSize).Result);
            }
            
        }

        /// <summary>
        /// Redirect to EditAccount page
        /// </summary>
        /// <param name="Id">Id of the user to be edited</param>
        /// <returns>the model containing the user's data</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAccount(string? Id)
        {
            try
            {   
                if (Id == null)
                {
                    return View();
                }
                var data = await _userAccountService.GetUpdateUserDTO(Id);
                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime} Failed to redirect to EditAccount page. Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Account");
            }
        }

        /// <summary>
        /// Edit an Account
        /// </summary>
        /// <param name="model">model containing the info to be change on an Account</param>
        /// <returns>To Account Index page if successfully edited the account, else shows errors</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditAccount(UpdateUser model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var work = await _userAccountService.UpdateUser(model);
                    if (!work.Result)
                    {
                        if (work.ErrorCode == ErrorType.Argument)
                        {
                            ModelState.AddModelError(string.Empty, work.Message);
                        }
                        _logger.LogError("Work Failed, {ErrorCode} {Message}", work.ErrorCode, work.Message);
                        return View();
                    }

                    _fileLogger.Log($"{LogType.DatabaseType}, {work.Message} \'{model?.Username}\', {User.Identity?.Name}", true);
                    return RedirectToAction("Index", "Account");
                }
                ModelState.AddModelError(string.Empty, ErrorType.Generic);
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to Edit Account Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Account");
            }

        }

        /// <summary>
        /// A confimation modal to delete a user
        /// </summary>
        /// <param name="Id">Id of the user to be deleted</param>
        /// <returns>The delete modal</returns>
        public async Task<IActionResult> DeleteModal(string? Id)
        {
            try
            {
                if (Id == null)
                {
                    return RedirectToAction("Index", "Account");
                }
                var data = await _userAccountService.GetUserById(Id);
                return PartialView("_DeleteModal", data);
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to load Delete Modal. Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Account");
            }
        }

        /// <summary>
        /// Delete a user Account
        /// </summary>
        /// <param name="model">model containing the user info</param>
        /// <returns>Returns to index page</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(UsersViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var work = await _userAccountService.DeleteUser(model.Id!);
                    if (!work.Result)
                        _logger.LogError(work.Message, work.ErrorCode);

                    _fileLogger.Log($"{LogType.DatabaseType}, {work.Message} \'{model?.UserName}\', {User.Identity?.Name}", true);

                    //If user delete his own account, for some stupid reasons
                    if (User.Identity?.Name == model?.UserName)
                        await _signInManager.SignOutAsync();

                    return RedirectToAction("Index", "Account");

                }
                return RedirectToAction("Index", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to delete. Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Account");
            }
        }

        /// <summary>
        /// The Login Page
        /// </summary>
        /// <returns>The Login Page</returns>
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Log-in the user
        /// </summary>
        /// <param name="model">model containing login informations</param>
        /// <returns>To landing page if successful login</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Login
                    var result = await _signInManager.PasswordSignInAsync(model.Username!, model.Password!, false, false);
                    if (result.Succeeded)
                    {
                        _fileLogger.Log($"{LogType.UserType}, Sign-In, {model.Username}", true);
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Invalid Username or Pasword");
                    return View(model);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to Login: {message}", DateTime.Now.ToString(), ex.Message);
                return View(model);
            }

            
        }

        /// <summary>
        /// The Create a new Account Page
        /// </summary>
        /// <returns>The Create a Account Page</returns>
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Creates a new Account
        /// </summary>
        /// <param name="model">model containing the account info</param>
        /// <returns>To Account Index page if successful</returns>
        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var work = await _userAccountService.CreateUser(model);
                    if (!work.Result)
                    {
                        if (work.ErrorCode == ErrorType.Argument)
                        {
                            ModelState.AddModelError(string.Empty, work.Message);
                        }
                        _logger.LogError("{datetime} Method CreateUser Failed: {errorcode}, {message}", DateTime.Now.ToString(), work.ErrorCode, work.Message);
                        return View();
                    }

                    _fileLogger.Log($"{LogType.DatabaseType}, {work.Message} \'{model?.Username}\', {User.Identity?.Name}", true);
                    return RedirectToAction("Index", "Account");
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to create new Account Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Account");
            }
        }

        /// <summary>
        /// Log out the current Signed in user
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                _fileLogger.Log($"{LogType.UserType}, Sign-Out, {User.Identity?.Name}", true);
                return RedirectToAction("Index", "Home");
            } 
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to Log-out user. Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Home");
            }
            
        }

        /// <summary>
        /// Show the account detail of the current logged in user
        /// </summary>
        /// <returns>The page Show the account detail of the current logged in user</returns>
        public async Task<IActionResult> Details()
        {
            try
            {
                var getUser = await _userManager.GetUserAsync(User);
                if (getUser == null) return RedirectToAction("Index", "Home");
                var Id = getUser.Id;
                var user = await _userAccountService.GetUserById(Id);
                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to show Account Detail. Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Show the Edit Detail page of the current logged in user
        /// </summary>
        /// <returns>Shows the Edit Detail page of the current logged in user</returns>
        public async Task<IActionResult> EditDetails()
        {
            try
            {
                var getUser = await _userManager.GetUserAsync(User);
                if (getUser == null) return RedirectToAction("Index", "Home");
                var Id = getUser.Id;
                var user = await _userAccountService.GetUserDetails(Id);
                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to show Edit Account Detail. Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Details", "Account");
            }
        }

        /// <summary>
        /// Edit the account details of the current logged in user
        /// </summary>
        /// <param name="model">model containing the user details</param>
        /// <returns>To Account Details page</returns>
        [HttpPost]
        public async Task<IActionResult> EditAccountDetails(UpdateUserDetails model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var work = await _userAccountService.UpdateUserDetails(model);
                    if (!work.Result)
                    {
                        if (work.ErrorCode == ErrorType.Argument)
                            ModelState.AddModelError(string.Empty, work.Message);
                        _logger.LogError("{datetime} Method UpdateUserDetails Failed, {ErrorCode} {Message}", DateTime.Now.ToString(), work.ErrorCode, work.Message);
                        return View("EditDetails", model);
                    }

                    _fileLogger.Log($"{LogType.UserType}, Changed Account Details, {User.Identity?.Name}", true);
                    return RedirectToAction("Details", "Account");
                }
                ModelState.AddModelError(string.Empty, ErrorType.Generic.ToString());
                return View("EditDetails", model);
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to Edit Account Details. Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Details", "Account");
            }
        }

        /// <summary>
        /// Block Unauthorized Access to certain pages
        /// </summary>
        /// <returns>To Landing page</returns>
        public ActionResult AccessDenied()
        {
            try
            {
                _logger.LogWarning("Blocked Unauthorized Access to part of a system. User: {user}", User.Identity?.Name);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to get username of blocked User. Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
