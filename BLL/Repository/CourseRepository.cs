using BLL.IRepository;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public List<Course> GetCoursesByInstructorId(int InstructorId)
        {
            List<Course> courses = new List<Course>();
            List<Course> InsCourses = new List<Course>();
            Instructor instructor = context.Instructors.Include(i => i.Ins).FirstOrDefault(i => i.InsId ==  InstructorId);
            
            if(instructor != null)
            {
                courses = context.Courses.Include(c => c.Ins).ToList();
            }
            foreach(Course course in courses) { 
                if(course.Ins != null && course.Ins.Contains(instructor))
                    InsCourses.Add(course);
            }
            return InsCourses;
        }




    }
}
