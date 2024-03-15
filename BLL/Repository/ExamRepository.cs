using BLL.IRepository;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repository
{
    public class ExamRepository:IExamRepository
    {

        private readonly ITIContext context;
        public ExamRepository(ITIContext _context)
        {
            context = _context;
        }

        public Exam GetCurrentExamByStudentId(int StudentId)
        {
            var currentExam = GetAllExamByStudentId(StudentId).FirstOrDefault(e => e.Date == DateOnly.FromDateTime(DateTime.Today));
            return currentExam;
        }

        public List<Exam> GetAllExamByStudentId(int StudentId)
        {
            var stdCourses = context.StudentCourses.Include(c=>c.Crs).Where(c => c.SId == StudentId).ToList();

            List<Exam> exams = new List<Exam>();
            foreach (var course in stdCourses)
            {
                var exam = context.Exams.FirstOrDefault(e => e.CrsId == course.CrsId);
                if (exam!=null)
                {
                    exams.Add(exam);
                }
            }

            return exams;
        }
        public List<Question> GetQuestionsByExamId()
        {
            return null;
        }
        public List<Choice> GetChoicesByQuestionId()
        {
            return null;
        }

    }
}
