using Microsoft.AspNetCore.Mvc;

namespace ExamSystemPL.Controllers
{
    public class InstructorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
