using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QFRMS.Data.DTOs;
using QFRMS.Data.Models;
using QFRMS.Data.ViewModels;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Utils;
using static QFRMS.Data.Constants;

namespace QFRMS.WebApp.Controllers
{
    [Authorize]
    public class MemoController : Controller
    {
        private readonly ILogger<MemoController> _logger;
        private readonly IFileLogger _fileLogger;
        private readonly IMemoService _memoService;

        public MemoController(ILogger<MemoController> logger, IFileLogger fileLogger, IMemoService memoService)
        {
            _logger = logger;
            _fileLogger = fileLogger;
            _memoService = memoService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var data = await _memoService.GetMemoAsync();
                if(data.File == null) data.Id = 0;
                return View(data);
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Memo Index Failed: {ex.Message}, {ex.InnerException}", true);
                Memo dummyData = new() { Id = 0};
                return View(dummyData);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UploadMemo(UploadMemo model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var work = await _memoService.UploadMemoAsync(model);
                    _fileLogger.Log(LogType.DatabaseType, $"{LogType.DatabaseType}, {work.Message} \'{model?.File.FileName}\', {User.Identity?.Name}", true);

                    //Add Admin to SeenUserTable
                    if (User.Identity == null) throw new ArgumentException("No User Logged in at the moment.");
                    var Name = User.Identity.Name ?? throw new ArgumentException("User has no name, wut.");
                    _ = await _memoService.HasSeenMemo(User.Identity.Name);

                    return RedirectToAction("Index", "Memo");
                }
                return RedirectToAction("Index", "Memo");
            }
            catch(ArgumentException ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Upload Memo Failed: {ex.GetType} {ex.Message}", true);
                return RedirectToAction("Index", "Memo");
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Upload Memo Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Memo");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UploadNewMemo() 
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Upload New Memo Page Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Memo");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadMemo()
        {
            try
            {
                return await _memoService.DownloadMemo();
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Download Memo Failed: {ex.Message}, {ex.InnerException}", true);
                return NotFound();
            }
        }

        // GET : DisplayMemoModal
        public IActionResult DisplayMemoModal()
        {
            try
            {
                return PartialView("_MemoModal");
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Display Memo Modal Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
