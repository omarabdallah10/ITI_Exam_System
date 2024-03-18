using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IRepository
{
    public interface ICourseRepository
    {
        public List<Course> GetAllCourses();
        public Course GetCourseById(int CourseId);
        public List<Course> GetCoursesByStudentId(int StudentId);
        public List<Exam> GetExamsByCourseId(int CourseId);

    }
}
