using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using QFRMS.Data.Models;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Utils;
using QFRMS.WebApp.Models;
using System.Diagnostics;
using static QFRMS.Data.Constants;
using Microsoft.IdentityModel.Tokens;
using QFRMS.Data.DTOs;

namespace QFRMS.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAboutService _aboutService;
        private readonly IMemoService _memoService;
        private readonly IFileLogger _fileLogger;

        public HomeController(ILogger<HomeController> logger, IAboutService aboutService, IMemoService memoService, IFileLogger fileLogger)
        {
            _logger = logger;
            _aboutService = aboutService;
            _memoService = memoService;
            _fileLogger = fileLogger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                //Check if a memo exist
                var memo = await _memoService.GetMemoAsync(null);
                if (memo.File == null)
                {
                    ViewData["HasSeenMemo"] = true; // Memo modal won't show
                }
                else if (User.Identity != null && User.Identity.Name != null)
                {
                    var Name = User.Identity.Name;
                    var HasSeenMemo = await _memoService.HasSeenMemo(Name);
                    if (HasSeenMemo)
                        ViewData["HasSeenMemo"] = true;
                    else
                        ViewData["HasSeenMemo"] = false;
                }
                var result = await _aboutService.GetHomePageArticlesVideosAsync();
                return View(result);
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Failed to get neccesary data for Memo notification: {ex.Message}, {ex.InnerException}", true);
                ViewData["HasSeenMemo"] = true;
                return View();
            }
        }

        /// <summary>
        /// Shows Information about the Institution
        /// </summary>
        /// <returns>The About page</returns>
        [Authorize]
        public async Task<IActionResult> About()
        {
            try
            {
                return View(await _aboutService.GetInstituteInfoAsync());
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"About Page Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Gives the function to edit the Institution Info
        /// </summary>
        /// <returns>The EditAbout Page</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAbout()
        {
            try
            { 
                return View(await _aboutService.GetInstituteInfoAsync());
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Edit About Page Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("About", "Home");
            }
        }

        /// <summary>
        /// Updates the Institution Info
        /// </summary>
        /// <param name="model">The model containing the updated Info</param>
        /// <returns>To About Page if successful</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInstituteInfo(InstituteInfo model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var work = await _aboutService.UpdateInstituteInfoAsync(model);
                    if(!work.Result)
                    {
                        _fileLogger.Log(LogType.ErrorType, $"Update Institute Info Failed: {work.ErrorCode} {work.Message}", true);
                        return View("EditAbout", model);
                    }
                    _fileLogger.Log(LogType.DatabaseType, $"{LogType.DatabaseType}, {work.Message}, {User.Identity?.Name}", true);
                    return RedirectToAction("About", "Home");
                }
                ModelState.AddModelError(string.Empty, "Please fill-up all the fields");
                return View("EditAbout", model);
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Update Institute Info Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("About", "Home");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> HomeSettings()
        {
            try
            {
                var result = await _aboutService.GetHomePageArticlesVideosAsync();
                return View(result);
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Home Settings Page Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("About", "Home");
            }
        }

        [Authorize(Roles = "Admin")]
        // Get : GetStudentGrades
        public IActionResult GetArticleVideoForm(string Id)
        {
            try
            {
                var result = _aboutService.GetUpdateArticleVideo(Id).Result;
                return PartialView("_UpdateModal", result);
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"GetArticleVideoForm Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("About", "Home");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateArticleVideo(UpdateArticleVideo model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var work = await _aboutService.UpdateHomePageArticlesVideoAsync(model);
                    _fileLogger.Log(LogType.DatabaseType, $"{LogType.DatabaseType}, {work.Message}, {User.Identity?.Name}", true);
                    return RedirectToAction("HomeSettings", "Home");
                }
                return RedirectToAction("HomeSettings", "Home");
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"UpdateArticleVideo Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("About", "Home");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteArticleVideo(string Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var work = await _aboutService.DeleteHomePageArticlesVideoAsync(Id);
                    _fileLogger.Log(LogType.DatabaseType, $"{LogType.DatabaseType}, {work.Message}, {User.Identity?.Name}", true);
                    return RedirectToAction("HomeSettings", "Home");
                }
                return RedirectToAction("HomeSettings", "Home");
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"UpdateArticleVideo Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("About", "Home");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
