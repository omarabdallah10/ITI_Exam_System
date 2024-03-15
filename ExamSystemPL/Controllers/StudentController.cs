using BLL.IRepository;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;


namespace ExamSystemPL.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IExamRepository examRepository;
        public StudentController(IStudentRepository _studentRepository, IExamRepository _examRepository)
        {
            studentRepository = _studentRepository;
            examRepository = _examRepository;
        }

        public IActionResult Index()
        {
            var stdId = 2;

            var stds= studentRepository.GetAllStudents();
            var exs = examRepository.GetAllExamByStudentId(stdId);
            var ex = examRepository.GetCurrentExamByStudentId(stdId);

            return View(stds);
        }

    }
}
