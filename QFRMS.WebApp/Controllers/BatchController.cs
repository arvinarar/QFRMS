using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using QFRMS.Data.DTOs;
using QFRMS.Data.Models;
using QFRMS.Data.ViewModels;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Services;
using QFRMS.Services.Utils;
using System.Runtime.Intrinsics.Arm;
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
                _fileLogger.Log(LogType.ErrorType, $"Batch Index Failed: {ex.Message}, {ex.InnerException}", true);
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
                _fileLogger.Log(LogType.ErrorType, $"Batch Search Failed: {ex.Message}, {ex.InnerException}", true);
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
                _fileLogger.Log(LogType.ErrorType, $"Batch Details Page Failed: {ex.Message}, {ex.InnerException}", true);
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
                _fileLogger.Log(LogType.ErrorType, $"Batch Edit Page Failed: {ex.Message}, {ex.InnerException}", true);
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
                _fileLogger.Log(LogType.ErrorType, $"GetBatchList Failed: {ex.Message}, {ex.InnerException}", true);
                var dummyList = new List<BatchCourseListViewModel>();
                return PartialView("_BatchCourseList", PaginatedList<BatchCourseListViewModel>.CreateAsync(dummyList, pageNumber ?? 1, _pageSize).Result);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(string? courseId)
        {
            try
            {
                var data = await _batchService.GetCreateBatchDTOAsync(courseId ?? null);
                return View(data);
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Batch Create Page Failed: {ex.Message}, {ex.InnerException}", true);
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
                        _fileLogger.Log(LogType.ErrorType, $"Create Batch Failed: {work.ErrorCode} {work.Message}", true);
                        return RedirectToAction("Create", "Batch", new { Id = model.CourseId });
                    }
                    _fileLogger.Log(LogType.DatabaseType, $"{LogType.DatabaseType}, {work.Message} \'{model?.RQMNumber?.ToUpperInvariant()}\', {User.Identity?.Name}", true);
                    if (model!.FromCoursePage)
                        return RedirectToAction("Details", "Course", new { Id = model.CourseId });
                    else
                        return RedirectToAction("Index", "Batch");
                }
                return RedirectToAction("Create", "Batch", new { Id = model.CourseId });
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Create Batch Failed: {ex.Message}, {ex.InnerException}", true);
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
                        _fileLogger.Log(LogType.ErrorType, $"Update Batch Failed: {work.ErrorCode} {work.Message}", true);
                        return RedirectToAction("Details", "Batch", new { model.Id, model.FromCoursePage });
                    }
                    _fileLogger.Log(LogType.DatabaseType, $"{LogType.DatabaseType}, {work.Message} \'{model?.RQMNumber?.ToUpperInvariant()}\', {User.Identity?.Name}", true);
                    return RedirectToAction("Details", "Batch", new { model!.Id, model.FromCoursePage });
                }
                return RedirectToAction("Details", "Batch", new { model.Id, model.FromCoursePage });
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Update Batch Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Batch");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBatch(BatchDetailViewModel model)
        {
            try
            {
                var work = await _batchService.DeleteBatchAsync(model.Id);
                if (!work.Result)
                    _fileLogger.Log(LogType.ErrorType, $"Delete Batch Failed: {work.ErrorCode} {work.Message}", true);
                else
                    _fileLogger.Log(LogType.DatabaseType, $"{LogType.DatabaseType}, {work.Message} \'{model?.RQMCode?.ToUpperInvariant()}\', {User.Identity?.Name}", true);
                
                if (model!.CourseId != null)
                    return RedirectToAction("Details", "Course", new { Id = model.CourseId });
                else
                    return RedirectToAction("Index", "Batch");
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Delete Batch Failed: {ex.Message}, {ex.InnerException}", true);
                if (model.CourseId != null)
                    return RedirectToAction("Details", "Course", new { Id = model.CourseId });
                else
                    return RedirectToAction("Index", "Batch");
            }
        }

        public async Task<IActionResult> ImportSheet(ImportSheet model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var work = await _batchService.AddStudentsFromCSV(model);
                    if (!work.Result)
                    {
                        _fileLogger.Log(LogType.ErrorType, $"Import Sheet Failed: {work.ErrorCode} {work.Message}", true);
                        return RedirectToAction("Details", "Batch", new { Id = model!.BatchId, model.FromCoursePage });
                    }
                    _fileLogger.Log(LogType.DatabaseType, $"{LogType.DatabaseType}, {work.Message}, {User.Identity?.Name}", true);
                    return RedirectToAction("Details", "Batch", new { Id = model!.BatchId, model.FromCoursePage });
                }
                return RedirectToAction("Details", "Batch", new { Id = model.BatchId, model.FromCoursePage });
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Import Sheet Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Batch");
            }
        }

        public async Task<IActionResult> GetDocument(string Id)
        {
            try
            {
                return await _batchService.GetDocument(Id);
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Get Document Failed: {ex.Message}, {ex.InnerException}", true);
                return NotFound();
            }
        }

        //GET : CheckIfAlreadyExist
        public async Task<bool> CheckIfAlreadyExist(string RQM)
        {
            try
            {
                var data = await _batchService.CheckIfAlreadyExist(RQM);
                return data;
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"CheckIfAlreadyExist Failed: {ex.Message}, {ex.InnerException}", true);
                return false;
            }
        }

        [Authorize(Roles = "Admin, Registrar")]
        public async Task<IActionResult> GenerateTerminalReport(string Id)
        {
            try
            {
                var TerminalReport = await _batchService.GenerateTerminalReport(Id, User.Identity?.Name);
                _fileLogger.Log(LogType.UserType, $"{LogType.UserType}, Generated Terminal Report for Batch \'{Id}\', {User.Identity?.Name}", true);

                return TerminalReport;
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Generate Terminal Report Failed: {ex.Message}, {ex.InnerException}", true);
                return NotFound();
            }
        }
    }
}
