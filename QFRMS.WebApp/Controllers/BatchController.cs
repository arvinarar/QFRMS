using Microsoft.AspNetCore.Mvc;

namespace QFRMS.WebApp.Controllers
{
    public class BatchController : Controller
    {
        private readonly ILogger<BatchController> _logger;

        public BatchController(ILogger<BatchController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
