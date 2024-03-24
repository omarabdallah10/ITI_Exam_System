using BLL.IRepository;
using BLL.ViewModels;
using DAL.Data;
using DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.Repository
{
    public class ExamRepository:IExamRepository
    {

        private readonly ITIContext context;
        public ExamRepository(ITIContext _context)
        {
            context = _context;
        }
        public Exam GetExamById(int ExamId)
        {
            var exam = context.Exams.Include(e=>e.QIds).Include(e => e.Crs).FirstOrDefault(e => e.ExId == ExamId);
            return exam;
        }

        public Exam GetCurrentExamByStudentId(int StudentId)
        {
            var currentExam = GetAllExamByStudentId(StudentId)
                                  .FirstOrDefault(e => e.Date == DateOnly.FromDateTime(DateTime.Today));

            return currentExam;
        }
        public int GenerateExamByCrsId(int CrsId, DateTime date, TimeSpan d)
        {
            //var generatedExam = context.Exams.FromSqlRaw("EXEC generate_exam {0}, {1}, {2}", CrsId, date, d).AsEnumerable().FirstOrDefault();
            //Console.WriteLine(generatedExam);

            var crsIdParam = new SqlParameter("@crs_id", CrsId);
            var dateParam = new SqlParameter("@exam_date", date);
            var durationParam = new SqlParameter("@duration", d);
            var newExamId = new SqlParameter("@new_exam_id", SqlDbType.Int);
            newExamId.Direction = ParameterDirection.Output;

            var result = context.Database.ExecuteSqlRaw(
                "EXEC generate_exam @crs_id, @exam_date, @duration, @new_exam_id output",
                crsIdParam, dateParam, durationParam, newExamId);

            var ExamId = new SqlParameter("@exam_id", (int)newExamId.Value);
            var resExam = context.Database.ExecuteSqlRaw(
                "EXEC get_exam @exam_id", ExamId);
            //Console.WriteLine(newExamId);
            Console.WriteLine(resExam);
            
            return (int)newExamId.Value;
        }
        public List<Exam> GetAllExamByStudentId(int StudentId)
        {
            var stdExamIds = context.StdExams
                                    .Include(e => e.Ex)
                                    .Where(s => s.StdId == StudentId)
                                    .Select(e => e.ExId)
                                    .Distinct()
                                    .ToList(); // Materialize the results by converting to a list

            List<Exam> exams = new List<Exam>();
            foreach (var item in stdExamIds)
            {
                var exam = context.Exams.FirstOrDefault(e => e.ExId == item);
                if (exam != null)
                {
                    exams.Add(exam);
                }
            }
            return exams;
        }

        public List<Question> GetQuestionsByExamId(int ExamId)
        {
            var exam = GetExamById(ExamId);
            var examQuestions = exam.QIds.ToList();
            for (int i=0; i<examQuestions.Count; i++)
            {
                examQuestions[i] = context.Questions.Include(q => q.Ches).FirstOrDefault(s => s.QId == examQuestions[i].QId);
            }
            return examQuestions;
        }

        public List<Choice> GetChoicesByQuestionId(int QuestId)
        {
            var Question = context.Questions.Include(q=>q.Ches).FirstOrDefault(s=>s.QId== QuestId);
            var QuestChoices = Question.Ches.ToList();
            return QuestChoices;
        }
        public Status SubmitStudentExam(StudentExamViewModel studentExamViewModel)
        {
            if (studentExamViewModel.QuestionsAnswers.Count>0)
            {
                foreach (var qAnswer in studentExamViewModel.QuestionsAnswers)
                {
                    var stdExam = new StdExam();
                    stdExam.StdId = studentExamViewModel.StdId;
                    stdExam.ExId = studentExamViewModel.ExamId;
                    stdExam.QId = qAnswer.Key;
                    stdExam.StdAnswer = qAnswer.Value;
                    context.StdExams.Add(stdExam);
                }
                context.SaveChanges();
                return Status.Success;
            }
            return Status.Failed;
        }
        public Tuple<int,int> CalculateTotalGrade(StudentExamViewModel studentExamViewModel)
        {
            if (studentExamViewModel.QuestionsAnswers.Count > 0)
            {
                studentExamViewModel.Exam = GetExamById(studentExamViewModel.ExamId);
                int totalGrade = 0;
                int overAllGrade = 0;
                foreach (var qAnswer in studentExamViewModel.QuestionsAnswers)
                {
                    var question = context.Questions.FirstOrDefault(q => q.QId == qAnswer.Key);
                    var questionModelAns = question.Answer;
                    overAllGrade += question.Score.Value;
                    if (questionModelAns == qAnswer.Value )
                    {
                        totalGrade += question.Score.Value;
                    }
                }
                SaveExamTotalScore(studentExamViewModel.ExamId, totalGrade);
                SaveStudentCourseGrade(studentExamViewModel, totalGrade);
                //context.SaveChanges();
                return new Tuple<int,int >(totalGrade,overAllGrade);
            }
            return new Tuple<int, int > (0, 0);
        }
        private void SaveExamTotalScore(int ExamId,int TotalScore)
        {
            var exam = GetExamById(ExamId);
            exam.TotalScore = TotalScore;
            context.SaveChanges();
        }
        private void SaveStudentCourseGrade(StudentExamViewModel studentExamViewModel,int totalGrade)
        {
            var stdCourse = context.StudentCourses.FirstOrDefault(c => c.SId == studentExamViewModel.StdId && c.CrsId == studentExamViewModel.Exam.CrsId);
            if (stdCourse != null)
            {
                stdCourse.Grade = totalGrade;
                context.SaveChanges();
            }
        }

        public bool IsStudentExamSubmitted(int StudentId, int ExamId)
        {
            var stdExam = context.StdExams.FirstOrDefault(e => e.StdId == StudentId && e.ExId == ExamId&&e.StdAnswer!=null);
            if (stdExam != null)
            {
                return true;
            }
            return false;
        }

        public bool IsExamTimeUp(int ExamId)
        {
            var exam = GetExamById(ExamId);
            if (exam.Date < DateOnly.FromDateTime(DateTime.Today))
            {
                return true;
            }
            return false;
        }

       public Exam GetExamByCourseId(int CourseId)
        {
            var exam = context.Exams.FirstOrDefault(e => e.CrsId == CourseId);
            return exam;
        }

        public int? GetStudentAnswer(int stdId, int qId, int exId)
        {
           var stdExam = context.StdExams.FirstOrDefault(e => e.StdId == stdId && e.ExId == exId && e.QId == qId);
            if (stdExam != null)
            {
                return stdExam.StdAnswer;
            }
            return null;
        }

        public void AssignExamToStudents(int examId, List<int> studentsIds)
        {
            try
            {
                foreach (var stdId in studentsIds)
                {
                    var examIdParam = new SqlParameter("@examId", examId);
                    var stdIdParam = new SqlParameter("@stdId", stdId);
                    context.Database.ExecuteSqlRaw("AssignExamToStudent @examId, @stdId",
                        examIdParam, stdIdParam);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
