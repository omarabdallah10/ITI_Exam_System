using Microsoft.AspNetCore.Authorization;
using BLL.IRepository;
using BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace ExamSystemPL.Controllers
{
    [Authorize(Roles = "instructor")]
    public class InstructorController : Controller
    {
        private readonly IExamRepository examRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ICourseRepository courseRepository;
        private readonly ITIContext context;
        public InstructorController(IExamRepository _examRepository, IAccountRepository _accountRepository, IStudentRepository _studentRepository, ITIContext _context, ICourseRepository _courseRepository)
        { 
            examRepository = _examRepository;
            accountRepository = _accountRepository;
            studentRepository = _studentRepository;
            courseRepository = _courseRepository;
            context = _context;
        }
        public IActionResult Index()
        {
            var teacherName = User.Identity.Name;
            var user = accountRepository.GetUserByName(teacherName);

            List<Course> courses = courseRepository.GetCoursesByInstructorId(user.UId);
            return View(courses);
        }

        [HttpPost]
        public IActionResult GenerateExam(int crsId, DateTime date, int hours, int minutes)
        {
            var generatedExamId = examRepository.GenerateExamByCrsId(crsId, date, new TimeSpan(hours, minutes, 0));

            Console.WriteLine(generatedExamId);

            var generatedExam = examRepository.GetExamById(generatedExamId);
            var questionsPopulated = examRepository.GetQuestionsByExamId(generatedExamId);

            generatedExam.QIds = questionsPopulated;
            Console.WriteLine(generatedExam.QIds);

            return View("GeneratedExam" ,generatedExam);
        }
        public IActionResult GenerateExam()
        {
            return View();
        }

        public IActionResult ExamCorrection(int exId, int stdId)
        {
            StudentExamViewModel studentExamVM = new StudentExamViewModel();
            var questions = examRepository.GetQuestionsByExamId(exId);

            studentExamVM.ExamId = exId;
            studentExamVM.Exam = examRepository.GetExamById(exId);
            studentExamVM.Questions = questions;
            studentExamVM.StdId = stdId;

            studentExamVM.QuestionsAnswers = new Dictionary<int, int?>();
            foreach (var question in questions)
            {
                var studentAnswer = examRepository.GetStudentAnswer(stdId, question.QId, exId);
                studentExamVM.QuestionsAnswers.Add(question.QId, studentAnswer);
            }

            return View(studentExamVM);
        }


        public IActionResult AssignExamToStudent(int examId)
        {
            var exam = examRepository.GetExamById(examId);
            var teacherName = User.Identity.Name;

            var user = accountRepository.GetUserByName(teacherName);

            int deptId = user.Instructor.DeptId.Value;

            var students = studentRepository.GetStudentsByDeptId(deptId);

            if (students == null)
                return View();

            ViewBag.students = students;
            return View(exam);
        }

        [HttpPost]
        public IActionResult AssignExamToStudent( int examId, string concatenatedStudentsIds)
        {
            var studentsIds = concatenatedStudentsIds.Split(",").Select(id => int.Parse(id)).ToList();
            foreach (var student in studentsIds)
            {
                Console.WriteLine(student);
            }
            examRepository.AssignExamToStudents(examId, studentsIds);
            return Content("done");
        }
    }
}
