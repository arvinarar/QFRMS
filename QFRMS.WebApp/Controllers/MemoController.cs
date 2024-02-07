using Microsoft.AspNetCore.Mvc;

namespace QFRMS.WebApp.Controllers
{
    public class MemoController : Controller
    {
        private readonly ILogger<MemoController> _logger;

        public MemoController(ILogger<MemoController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
