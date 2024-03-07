using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using QFRMS.Data.Models;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Utils;
using QFRMS.WebApp.Models;
using System.Diagnostics;
using static QFRMS.Data.Constants;

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
                if (User.Identity != null && User.Identity.Name != null)
                {
                    var Name = User.Identity.Name;
                    var HasSeenMemo = await _memoService.HasSeenMemo(Name);
                    if (HasSeenMemo)
                        ViewData["HasSeenMemo"] = true;
                    else
                        ViewData["HasSeenMemo"] = false;
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to get neccesary data for Memo notification: {message}", DateTime.Now.ToString(), ex.Message);
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
                        _logger.LogError("Work Failed, {ErrorCode} {Message}", work.ErrorCode, work.Message);
                        return View("EditAbout", model);
                    }
                    _fileLogger.Log($"{LogType.DatabaseType}, {work.Message}, {User.Identity?.Name}", true);
                    return RedirectToAction("About", "Home");
                }
                ModelState.AddModelError(string.Empty, "Please fill-up all the fields");
                return View("EditAbout", model);
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to Update Insitute Info: {message}", DateTime.Now.ToString(), ex.Message);
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
