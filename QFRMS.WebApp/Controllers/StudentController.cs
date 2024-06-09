using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QFRMS.Data.DTOs;
using QFRMS.Data.ViewModels;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Services;
using QFRMS.Services.Utils;
using System.Diagnostics;
using System.Reflection;
using static QFRMS.Data.Constants;

namespace QFRMS.WebApp.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IStudentService _service;
        private readonly IFileLogger _fileLogger;
        private readonly ILogger<StudentController> _logger;
        private readonly int _pageSize = 5;

        public StudentController(IStudentService studentService, IFileLogger fileLogger, ILogger<StudentController> logger)
        {
            _service = studentService;
            _fileLogger = fileLogger;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            try
            {
                var data = await _service.GetStudentListAsync(User.IsInRole("Trainor") ? User.Identity!.Name : null);
                return View(await PaginatedList<StudentListViewModel>.CreateAsync(data, pageNumber ?? 1, 12));
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "An error has occured when trying to show students. Please contact administrator if this problem persists.";
                _fileLogger.Log(LogType.ErrorType, $"Student Index Failed: {ex.Message}, {ex.InnerException}", true);
                var dummyList = new List<StudentListViewModel>();
                return View(await PaginatedList<StudentListViewModel>.CreateAsync(dummyList, pageNumber ?? 1, 12));
            }
        }

        // GET : Search
        public PartialViewResult Search(string searchType, string searchInput, int? pageNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(searchInput))
                {
                    var result = _service.GetStudentListAsync(User.IsInRole("Trainor") ? User.Identity!.Name : null).Result;
                    return PartialView("_StudentList", PaginatedList<StudentListViewModel>.CreateAsync(result, pageNumber ?? 1, 12).Result);
                }
                else
                {
                    var result = _service.SearchStudentListAsync(searchType, searchInput, User.IsInRole("Trainor") ? User.Identity!.Name : null).Result;
                    return PartialView("_StudentList", PaginatedList<StudentListViewModel>.CreateAsync(result, pageNumber ?? 1, 12).Result);
                }
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"Student Search Failed: {ex.Message}, {ex.InnerException}", true);
                var dummyList = new List<StudentListViewModel>();
                return PartialView("_StudentList", PaginatedList<StudentListViewModel>.CreateAsync(dummyList, pageNumber ?? 1, 12).Result);
            }
        }

        // Get : GetStudentList
        public PartialViewResult GetStudentList(string batchId, string searchType, string searchInput, int? pageNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(searchInput))
                {
                    var result = _service.SearchStudentBatchListAsync(batchId, "n/a", "n/a").Result;
                    return PartialView("_StudentBatchList", PaginatedList<StudentListViewModel>.CreateAsync(result, pageNumber ?? 1, _pageSize).Result);
                }
                else
                {
                    var result = _service.SearchStudentBatchListAsync(batchId, searchType, searchInput).Result;
                    return PartialView("_StudentBatchList", PaginatedList<StudentListViewModel>.CreateAsync(result, pageNumber ?? 1, _pageSize).Result);
                }
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"StudentList Search Index Failed: {ex.Message}, {ex.InnerException}", true);
                var dummyList = new List<StudentListViewModel>();
                return PartialView("_StudentBatchList", PaginatedList<StudentListViewModel>.CreateAsync(dummyList, pageNumber ?? 1, _pageSize).Result);
            }
        }

        [Authorize(Roles = "Admin, Registrar")]
        public async Task<IActionResult> Create(string? BatchId, bool FromCoursePage = false)
        {
            try
            {
                var data = await _service.CreateStudentDTOAsync(BatchId ?? null, FromCoursePage);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Failed to show create student page. Please contact administrator if this problem persists.";
                _fileLogger.Log(LogType.ErrorType, $"Create Student Page Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Student");
            }
        }

        [Authorize(Roles = "Admin, Registrar")]
        public async Task<IActionResult> Edit(string ULI, string? BatchId, bool FromCoursePage = false)
        {
            try
            {
                var data = await _service.EditStudentDTOAsync(ULI, BatchId ?? null, FromCoursePage);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Failed to show edit student page. Please contact administrator if this problem persists.";
                _fileLogger.Log(LogType.ErrorType, $"Edit Student Page Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Student");
            }
        }

        [Authorize(Roles = "Admin, Registrar")]
        [HttpPost]
        public async Task<IActionResult> EnrollStudent(EnrollStudent model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var work = await _service.EnrollStudentAsync(model);
                    if (!work.Result)
                    {
                        if (work.ErrorCode == ErrorType.Argument)
                        {
                            ModelState.AddModelError(string.Empty, work.Message);
                        }
                        TempData["Failed"] = $"Failed to enroll student, Error: {work.Message}";
                        _fileLogger.Log(LogType.ErrorType, $"Enroll Student Failed: {work.ErrorCode} {work.Message}", true);
                        return RedirectToAction("Create", "Student", new { Id = model.BatchId, FromCoursePage = model.FromCoursePage });
                    }
                    TempData["Success"] = work.Message;
                    _fileLogger.Log(LogType.DatabaseType, $"{LogType.DatabaseType}, {work.Message}, {User.Identity?.Name}", true);
                    if (!model!.FromStudentsPage)
                        return RedirectToAction("Details", "Batch", new { Id = model.BatchId, FromCoursePage = model.FromCoursePage });
                    else
                        return RedirectToAction("Index", "Student");
                }
                TempData["Failed"] = "Failed to enroll student. Please contact administrator if this problem persists.";
                return RedirectToAction("Create", "Student", new { Id = model.BatchId, FromCoursePage = model.FromCoursePage });
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Failed to enroll student. Please contact administrator if this problem persists.";
                _fileLogger.Log(LogType.ErrorType, $"Enroll Student Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Student");
            }
        }

        [Authorize(Roles = "Admin, Registrar")]
        [HttpPost]
        public async Task<IActionResult> EditStudent(EditStudent model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var work = await _service.EditStudentAsync(model);
                    if (!work.Result)
                    {
                        TempData["Failed"] = $"Failed to edit student, Error: {work.Message}";
                        _fileLogger.Log(LogType.ErrorType, $"Edit Student Failed: {work.ErrorCode} {work.Message}", true);
                        return RedirectToAction("Index", "Student");
                    }
                    TempData["Success"] = work.Message;
                    _fileLogger.Log(LogType.DatabaseType, $"{LogType.DatabaseType}, {work.Message}, {User.Identity?.Name}", true);
                    if (!model!.FromStudentsPage)
                        return RedirectToAction("Details", "Batch", new { Id = model.BatchId, FromCoursePage = model.FromCoursePage });
                    else
                        return RedirectToAction("Index", "Student");
                }
                TempData["Failed"] = "Failed to edit student. Please contact administrator if this problem persists.";
                return RedirectToAction("Index", "Student");
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Failed to edit student. Please contact administrator if this problem persists.";
                _fileLogger.Log(LogType.ErrorType, $"Edit Student Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Student");
            }
        }

        [Authorize(Roles = "Admin, Registrar")]
        [HttpPost]
        public async Task<IActionResult> DeleteStudent(StudentDetailViewModel model)
        {
            try
            {
                var work = await _service.DeleteStudentAsync(model.ULI);
                TempData["Success"] = work.Message;
                _fileLogger.Log(LogType.DatabaseType, $"{LogType.DatabaseType}, {work.Message}, {User.Identity?.Name}", true);
                if (!model!.FromStudentsPage)
                    return RedirectToAction("Details", "Batch", new { Id = model.BatchId, FromCoursePage = model.FromCoursePage });
                else
                    return RedirectToAction("Index", "Student");
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Failed to delete student. Please contact administrator if this problem persists.";
                _fileLogger.Log(LogType.ErrorType, $"Delete Student Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Student");
            }
        }

        //GET : CheckIfAlreadyExist
        public async Task<bool> CheckIfAlreadyExist(string ULI)
        {
            try
            {
                var data = await _service.CheckIfAlreadyExist(ULI);
                return data;
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"CheckIfAlreadyExist Failed: {ex.Message}, {ex.InnerException}", true);
                return false;
            }
        }

        // GET : GetStudentDetail
        public IActionResult GetStudentDetail(string ULI, string? batchId, string? fromCoursePage)
        {
            try
            {
                bool flag = false;
                if (!fromCoursePage.IsNullOrEmpty())
                    if (fromCoursePage == "True")
                        flag = true;
                var result = _service.GetStudentDetail(ULI, batchId, flag).Result;
                return PartialView("_StudentDetails", result);
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"GetStudentDetail Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Student");
            }
        }

        // Get : GetStudentGrades
        public IActionResult GetStudentGrades(string batchId, string userRole, string? fromCoursePage)
        {
            try
            {
                bool flag = false;
                if (!fromCoursePage.IsNullOrEmpty())
                    if (fromCoursePage == "True")
                        flag = true;
                var result = _service.GetStudentGrades(batchId, userRole, flag).Result;
                return PartialView("_StudentGradesList", result);
            }
            catch (Exception ex)
            {
                _fileLogger.Log(LogType.ErrorType, $"GetStudentGrade Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Student");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGrades(StudentGradesList model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var work = await _service.UpdateGrades(model);
                    TempData["Success"] = work.Message;
                    _fileLogger.Log(LogType.DatabaseType, $"{LogType.DatabaseType}, {work.Message}, {User.Identity?.Name}", true);
                }
                return RedirectToAction("Details", "Batch", new { Id = model.BatchId, FromCoursePage = model.FromCoursePage });
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Failed to update grades or statuses. Please contact administrator if this problem persists.";
                _fileLogger.Log(LogType.ErrorType, $"UpdateGrades Failed: {ex.Message}, {ex.InnerException}", true);
                return RedirectToAction("Index", "Student");
            }
        }
    }
}
