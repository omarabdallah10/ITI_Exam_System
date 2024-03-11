using Microsoft.AspNetCore.Mvc;

namespace ExamSystemPL.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
