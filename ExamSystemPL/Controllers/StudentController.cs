using BLL.IRepository;
using BLL.ViewModels;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ExamSystemPL.Controllers
{
    [Authorize(Roles = "student")]
    public class StudentController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IExamRepository examRepository;
        private readonly ICourseRepository courseRepository;
        public StudentController(IStudentRepository _studentRepository, IExamRepository _examRepository, ICourseRepository _courseRepository)
        {
            studentRepository = _studentRepository;
            examRepository = _examRepository;
            courseRepository = _courseRepository;
        }
       
        public IActionResult Index()
        {


            var stdId = GetStudentBy();
            var student = studentRepository.GetStudentById(stdId);
            ViewBag.currentStudent = student;
            var currentExam = examRepository.GetCurrentExamByStudentId(stdId);
            if (currentExam != null)
            {
                bool isExamSubmittedBefore = examRepository.IsStudentExamSubmitted(stdId, currentExam.ExId);

                if (isExamSubmittedBefore)
                {

                    return View();
                }
                return View(currentExam);
            }
            return View();
        }

        public IActionResult TakeExam(int id)
        {
            var stdId = GetStudentBy();
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

        public IActionResult SeeStudentExam(int crsId)
        {
            var stdId = GetStudentBy();
            var exam = examRepository.GetExamByCourseId(crsId);
            var questions = examRepository.GetQuestionsByExamId(exam.ExId);

            StudentExamViewModel studentExamVM = new StudentExamViewModel();
            var currentExam = examRepository.GetCurrentExamByStudentId(stdId);
            studentExamVM.Exam = exam;
            studentExamVM.StdId = stdId;
            studentExamVM.ExamId = exam.ExId;
            studentExamVM.Questions = questions;

            studentExamVM.QuestionsAnswers = new Dictionary<int, int?>();
            foreach (var question in questions)
            {
                var studentAnswer = examRepository.GetStudentAnswer(stdId, question.QId, exam.ExId);
                studentExamVM.QuestionsAnswers.Add(question.QId, studentAnswer);
            }
            return View(studentExamVM);
        }

        [HttpPost]
        public IActionResult SubmitExam(StudentExamViewModel studentExamViewModel)
        {
            //var status = examRepository.SubmitStudentExam(studentExamViewModel);
          
                // View Grade After Correction

                Tuple<int, int> totalgrades = examRepository.CalculateTotalGrade(studentExamViewModel);

                return View(nameof(ShowExamGrade), totalgrades);
            
            // Return To Index
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetCourses()
        {
            var stdId = GetStudentBy();
            var stdcourses = courseRepository.GetCoursesByStudentId(stdId);
            return View(stdcourses);
        }

        public IActionResult ShowExamGrade(Tuple<int, int> totalgrade)
        {
            return View(totalgrade);
        }
        private int GetStudentBy()
        {
            var userName = HttpContext.User.Identity.Name;
            var stdId = studentRepository.GetStudentByIdByName(userName);
            return stdId;
        }

    }
}
