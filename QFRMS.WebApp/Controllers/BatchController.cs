using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QFRMS.Data.DTOs;
using QFRMS.Data.Models;
using QFRMS.Data.ViewModels;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Services;
using QFRMS.Services.Utils;
using static QFRMS.Data.Constants;

namespace QFRMS.WebApp.Controllers
{
    [Authorize]
    public class BatchController : Controller
    {
        private readonly IBatchService _batchService;
        private readonly IFileLogger _fileLogger;
        private readonly ILogger<BatchController> _logger;
        private readonly int _pageSize = 6;

        public BatchController(IBatchService batchService, IFileLogger fileLogger, ILogger<BatchController> logger)
        {
            _batchService = batchService;
            _fileLogger = fileLogger;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            try
            {
                var data = await _batchService.GetBatchListAsync(User.IsInRole("Trainor") ? User.Identity!.Name : null);
                return View(await PaginatedList<BatchListViewModel>.CreateAsync(data, pageNumber ?? 1, 10));
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to retrieve courses. Error: {Message}", DateTime.Now.ToString(), ex.Message);
                var dummyList = new List<BatchListViewModel>();
                return View(await PaginatedList<BatchListViewModel>.CreateAsync(dummyList, pageNumber ?? 1, 10));
            }
        }

        // GET : Search
        public PartialViewResult Search(string searchType, string searchInput, int? pageNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(searchInput))
                {
                    var result = _batchService.GetBatchListAsync(User.IsInRole("Trainor") ? User.Identity!.Name : null).Result;
                    return PartialView("_BatchList", PaginatedList<BatchListViewModel>.CreateAsync(result, pageNumber ?? 1, 10).Result);
                }
                else
                {
                    var result = _batchService.SearchBatchListAsync(searchType, searchInput, User.IsInRole("Trainor") ? User.Identity!.Name : null).Result;
                    return PartialView("_BatchList", PaginatedList<BatchListViewModel>.CreateAsync(result, pageNumber ?? 1, 10).Result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Search Failed. Error: {Message}", DateTime.Now.ToString(), ex.Message);
                var dummyList = new List<BatchListViewModel>();
                return PartialView("_BatchList", PaginatedList<BatchListViewModel>.CreateAsync(dummyList, pageNumber ?? 1, 10).Result);
            }
        }

        public async Task<IActionResult> Details(string Id, bool FromCoursePage = false)
        {
            try
            {
                var data = await _batchService.GetBatchDetailAsync(Id, FromCoursePage);
                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to get Course Detail, Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Course");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string Id, bool FromCoursePage = false)
        {
            try
            {
                var data = await _batchService.GetUpdateBatchDTOAsync(Id, FromCoursePage);
                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to get Course Edit View, Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Course");
            }
        }

        // Get : GetBatchList
        public PartialViewResult GetBatchList(string courseId, string searchType, string searchInput, int? pageNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(searchInput))
                {
                    var result = _batchService.SearchBatchCourseListAsync(courseId, "n/a", "n/a").Result;
                    return PartialView("_BatchCourseList", PaginatedList<BatchCourseListViewModel>.CreateAsync(result, pageNumber ?? 1, _pageSize).Result);
                }
                else
                {
                    var result = _batchService.SearchBatchCourseListAsync(courseId, searchType, searchInput).Result;
                    return PartialView("_BatchCourseList", PaginatedList<BatchCourseListViewModel>.CreateAsync(result, pageNumber ?? 1, _pageSize).Result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Search Failed. Error: {Message}", DateTime.Now.ToString(), ex.Message);
                var dummyList = new List<BatchCourseListViewModel>();
                return PartialView("_BatchCourseList", PaginatedList<BatchCourseListViewModel>.CreateAsync(dummyList, pageNumber ?? 1, _pageSize).Result);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(string? courseId)
        {
            try
            {
                if (TempData.ContainsKey("ErrorMessage"))
                {
                    ModelState.AddModelError(String.Empty, TempData["ErrorMessage"].ToString());
                }
                var data = await _batchService.GetCreateBatchDTOAsync(courseId ?? null);
                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to load Create Batch Page, Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Batch");
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBatch(CreateBatch model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var work = await _batchService.AddBatchAsync(model);
                    if (!work.Result)
                    {
                        if (work.ErrorCode == ErrorType.Argument)
                        {
                            ModelState.AddModelError(string.Empty, work.Message);
                        }
                        _logger.LogError("{datetime} Method Create Failed: {errorcode}, {message}", DateTime.Now.ToString(), work.ErrorCode, work.Message);
                        TempData["ErrorMessage"] = work.Message;
                        return RedirectToAction("Create", "Batch", new { Id = model.CourseId });
                    }
                    _fileLogger.Log($"{LogType.DatabaseType}, {work.Message} \'{model?.RQMNumber?.ToUpperInvariant()}\', {User.Identity?.Name}", true);
                    if (model!.FromCoursePage)
                        return RedirectToAction("Details", "Course", new { Id = model.CourseId });
                    else
                        return RedirectToAction("Index", "Batch");
                }
                return RedirectToAction("Create", "Batch", new { Id = model.CourseId });
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to Create Batch, Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Batch");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateBatch(UpdateBatch model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var work = await _batchService.UpdateBatchAsync(model);
                    if(!work.Result)
                    {
                        _logger.LogError("{datetime} Method Update Failed: {errorcode}, {message}", DateTime.Now.ToString(), work.ErrorCode, work.Message);
                        return RedirectToAction("Details", "Batch", new { model.Id, model.FromCoursePage });
                    }
                    _fileLogger.Log($"{LogType.DatabaseType}, {work.Message} \'{model?.RQMNumber?.ToUpperInvariant()}\', {User.Identity?.Name}", true);
                    return RedirectToAction("Details", "Batch", new { model!.Id, model.FromCoursePage });
                }
                return RedirectToAction("Details", "Batch", new { model.Id, model.FromCoursePage });
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to Update Batch, Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Batch");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBatch(BatchDetailViewModel model)
        {
            try
            {
                var work = await _batchService.DeleteBatchAsync(model.Id);
                if (!work.Result) throw new Exception("Work Failed");

                _fileLogger.Log($"{LogType.DatabaseType}, {work.Message} \'{model?.RQMCode?.ToUpperInvariant()}\', {User.Identity?.Name}", true);
                
                if (model!.CourseId != null)
                    return RedirectToAction("Details", "Course", new { Id = model.CourseId });
                else
                    return RedirectToAction("Index", "Batch");
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to delete. Error: {message}", DateTime.Now.ToString(), ex.Message);
                if (model.CourseId != null)
                    return RedirectToAction("Details", "Course", new { Id = model.CourseId });
                else
                    return RedirectToAction("Index", "Batch");
            }
        }

        public async Task<FileContentResult?> GetDocument(string Id)
        {
            try
            {
                return await _batchService.GetDocument(Id) ?? throw new Exception("No Document Found");
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to get document, Error: {message}", DateTime.Now.ToString(), ex.Message);
                return null;
            }
        }
    }
}
