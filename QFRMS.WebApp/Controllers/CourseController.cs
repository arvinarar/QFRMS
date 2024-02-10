using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QFRMS.Data.Models;
using QFRMS.Data.ViewModels;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Utils;
using static QFRMS.Data.Constants;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QFRMS.WebApp.Controllers
{
    [Authorize(Roles = "Admin, Registrar")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IFileLogger _fileLogger;
        private readonly ILogger<CourseController> _logger;
        private readonly int _pageSize = 10;

        public CourseController(ICourseService courseService, IFileLogger fileLogger, ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _fileLogger = fileLogger;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            try
            {
                var data = await _courseService.GetCourseListAsync();
                return View(await PaginatedList<CourseListViewModel>.CreateAsync(data, pageNumber ?? 1, _pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to retrieve courses. Error: {Message}", DateTime.Now.ToString(), ex.Message);
                var dummyList = new List<CourseListViewModel>();
                return View(await PaginatedList<CourseListViewModel>.CreateAsync(dummyList, pageNumber ?? 1, _pageSize));
            }
        }

        // GET : Search
        public PartialViewResult Search(string searchType, string searchInput, int? pageNumber)
        {
            try
            {
                if(string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(searchInput))
                {
                    var result = _courseService.GetCourseListAsync().Result;
                    return PartialView("_CourseList", PaginatedList<CourseListViewModel>.CreateAsync(result, pageNumber ?? 1, _pageSize).Result);
                }
                else
                {
                    var result = _courseService.SearchCourseListAsync(searchType, searchInput).Result;
                    return PartialView("_CourseList", PaginatedList<CourseListViewModel>.CreateAsync(result, pageNumber ?? 1, _pageSize).Result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Search Failed. Error: {Message}", DateTime.Now.ToString(), ex.Message);
                var dummyList = new List<CourseListViewModel>();
                return PartialView("_CourseList", PaginatedList<CourseListViewModel>.CreateAsync(dummyList, pageNumber ?? 1, _pageSize).Result);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Course model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var work = await _courseService.AddCourseAsync(model);
                    if(!work.Result)
                    {
                        if (work.ErrorCode == ErrorType.Argument)
                        {
                            ModelState.AddModelError(string.Empty, work.Message);
                        }
                        _logger.LogError("{datetime} Method Create Failed: {errorcode}, {message}", DateTime.Now.ToString(), work.ErrorCode, work.Message);
                        return View();
                    }

                    _fileLogger.Log($"{LogType.DatabaseType}, {work.Message} \'{model?.ProgramTitle}\', {User.Identity?.Name}", true);
                    return RedirectToAction("Index", "Course");
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to create new Account, Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Course");
            }
        }

        public async Task<IActionResult> Details(string Id)
        {
            try
            {
                var data = await _courseService.GetCourseDetailAsync(Id);
                return View(data);
            }
            catch(Exception ex)
            {
                _logger.LogError("{datetime}: Failed to get Course Detail, Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Course");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string Id)
        {
            try
            {
                var data = await _courseService.GetCourseEditViewAsync(Id);
                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("{datetime}: Failed to get Course Edit View, Error: {message}", DateTime.Now.ToString(), ex.Message);
                return RedirectToAction("Index", "Course");
            }
        }
    }
}
