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
        private readonly int _pageSize = 8;

        public MemoController(ILogger<MemoController> logger, IFileLogger fileLogger, IMemoService memoService)
        {
            _logger = logger;
            _fileLogger = fileLogger;
            _memoService = memoService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var data = await _memoService.GetMemoAsync(null);
                if(data.File == null) data.Id = 0;
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "An error has occured when trying to show memos. Please contact administrator if this problem persists.";
                _fileLogger.Log(LogType.ErrorType, $"Memo Index Failed: {ex.Message}, {ex.InnerException}", true);
                Memo dummyData = new() { Id = 0};
                return View(dummyData);
            }
        }

        // Get : GetMemoList
        public PartialViewResult GetMemoList(int? pageNumber)
        {
            try
            {
                var result = _memoService.GetMemoList().Result;
                return PartialView("_MemoList", PaginatedList<MemoListViewModel>.CreateAsync(result, pageNumber ?? 1, _pageSize).Result);
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"MemoList show Failed: {ex.Message}, {ex.InnerException}", true);
                var dummyList = new List<MemoListViewModel>();
                return PartialView("_MemoList", PaginatedList<MemoListViewModel>.CreateAsync(dummyList, pageNumber ?? 1, _pageSize).Result);
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
                    TempData["Success"] = work.Message;
                    _fileLogger.Log(LogType.DatabaseType, $"{LogType.DatabaseType}, {work.Message} \'{model?.File.FileName}\', {User.Identity?.Name}", true);

                    //Add Admin to SeenUserTable
                    if (User.Identity == null) throw new ArgumentException("No User Logged in at the moment.");
                    var Name = User.Identity.Name ?? throw new ArgumentException("User has no name, wut.");
                    _ = await _memoService.HasSeenMemo(User.Identity.Name);

                    return RedirectToAction("Index", "Memo");
                }
                TempData["Failed"] = "Upload memo failed, see logs for details.";
                return RedirectToAction("Index", "Memo");
            }
            catch(ArgumentException ex)
            {
                TempData["Failed"] = $"Resetting seen users failed, Error: {ex.Message}";
                _fileLogger.Log(LogType.ErrorType, $"Seen user reset failed: {ex.GetType} {ex.Message}", true);
                return RedirectToAction("Index", "Memo");
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Upload memo failed, see logs for details.";
                _fileLogger.Log(LogType.ErrorType, $"Upload Memo Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Memo");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteMemo(MemoListViewModel model)
        {
            try
            {
                var work = await _memoService.DeleteMemoAsync(model.Id);
                TempData["Success"] = work.Message;
                _fileLogger.Log(LogType.DatabaseType, $"{LogType.DatabaseType}, {work.Message}, {User.Identity?.Name}", true);
                return RedirectToAction("Index", "Memo");
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Delete memo failed, see logs for details.";
                _fileLogger.Log(LogType.ErrorType, $"Delete Memo Failed: {ex.Message}, {ex.InnerException}", true);
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
        public async Task<IActionResult> DownloadMemo(int id)
        {
            try
            {
                return await _memoService.DownloadMemo(id);
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Download Memo Failed: {ex.Message}, {ex.InnerException}", true);
                return NotFound();
            }
        }

        // GET : DisplayMemoModal
        public IActionResult DisplayMemoModal(int? id)
        {
            try
            {
                var data = _memoService.GetMemoAsync(id).Result;
                return PartialView("_MemoModal", new Memo { Id = id ?? data.Id });
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Display Memo Modal Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
