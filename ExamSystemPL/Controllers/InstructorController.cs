using Microsoft.AspNetCore.Authorization;
using BLL.IRepository;
using BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystemPL.Controllers
{
    [Authorize(Roles = "instructor")]
    public class InstructorController : Controller
    {
        private readonly IExamRepository examRepository;
        public InstructorController(IExamRepository _examRepository)
        { 
            examRepository = _examRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GenerateExam()
        {
            var generatedExam = examRepository.GenerateExamByCrsId(1, DateTime.Now,new TimeSpan(1, 40, 0));
            Console.WriteLine(generatedExam);
            StudentExamViewModel studentExamVM = new StudentExamViewModel();

            return View();
        }
    }
}
