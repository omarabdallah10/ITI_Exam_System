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
    public class CourseRepository:ICourseRepository
    {
        private readonly ITIContext context;
        public CourseRepository(ITIContext _context)
        {
            context = _context;
        }
        public List<Course> GetAllCourses()
        {
            return context.Courses.Include(c=>c.StudentCourses).ToList();
        }
        public Course GetCourseById(int CourseId)
        {
            return context.Courses.Include(c=>c.Exams).FirstOrDefault(c => c.CrsId == CourseId);
        }
        public List<StudentCourse> GetCoursesByStudentId(int StudentId)
        {
            return context.StudentCourses.Include(sc => sc.Crs).Where(sc => sc.SId == StudentId).ToList();
        }
        public List<Exam> GetExamsByCourseId(int CourseId)
        {
            return context.Exams.Where(e => e.CrsId == CourseId).ToList();
        }




    }
}
