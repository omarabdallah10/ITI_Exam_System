using BLL.IRepository;
using BLL.ViewModels;
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
            //ViewBag.ExamData = examRepository.GetCurrentExamByStudentId(stdId);
            var questions = examRepository.GetQuestionsByExamId(id);
            

            StudentExamViewModel studentExamVM = new StudentExamViewModel();
            var currentExam = examRepository.GetCurrentExamByStudentId(stdId);
            studentExamVM.Exam = currentExam;
            studentExamVM.StdId = stdId;
            studentExamVM.ExamId = currentExam.ExId;
            studentExamVM.Questions = questions;

            studentExamVM.QuestionsAnswers = new Dictionary<int, int?>();

            foreach (var question in questions)
            {
                studentExamVM.QuestionsAnswers.Add(question.QId, null);
            }

            return View(studentExamVM);
        }

        [HttpPost]
        public IActionResult SubmitExam(StudentExamViewModel studentExamViewModel)
        {
            var status = examRepository.SubmitStudentExam(studentExamViewModel);
            if (status == BLL.Status.Success)
            {
                // View Grade After Correction

                Tuple<int,int> totalgrades = examRepository.CalculateTotalGrade(studentExamViewModel);

                return View(nameof(ShowExamGrade), totalgrades);
            }
            // Return To Index
            return RedirectToAction(nameof(Index));
            

        }

        public IActionResult ShowExamGrade(Tuple<int, int> totalgrade)
        {
            return View(totalgrade);
        }

    }
}
