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
            var student = studentRepository.GetStudentById(stdId);
            ViewBag.currentStudent= student;
            var currentExam = examRepository.GetCurrentExamByStudentId(stdId);

            //var Exams = examRepository.GetAllExamByStudentId(stdId);
            
            //var questionChoices =examRepository.GetChoicesByQuestionId(1);

            return View(currentExam);
        }

        public IActionResult TakeExam(int id)
        {
            var stdId = 2;
            ViewBag.ExamData = examRepository.GetCurrentExamByStudentId(stdId);
            var questions = examRepository.GetQuestionsByExamId(id);
            return View(questions);
        }


    }
}
