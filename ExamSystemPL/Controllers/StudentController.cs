using BLL.IRepository;
using Microsoft.AspNetCore.Mvc;


namespace ExamSystemPL.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository studentRepository;
        public StudentController(IStudentRepository _studentRepository)
        {
            studentRepository = _studentRepository;
        }

        public IActionResult Index()
        {
            var stds= studentRepository.GetAllStudents();

            return View(stds);
        }
    }
}
