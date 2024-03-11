using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IRepository
{
    public interface IStudentRepository
    {
        public List<Student> GetAllStudents();
        public Student GetStudentById(int id);
        public Status DeleteStudentById(int id);
        public Status CreateStudent(Student student);
        public Status UpdateStudent(int id, Student student);
    }
}
