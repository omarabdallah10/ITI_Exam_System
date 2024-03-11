using BLL.IRepository;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repository
{
    public class StudentRepository : IStudentRepository
    {
        public Status CreateStudent(Student student)
        {
            throw new NotImplementedException();
        }

        public Status DeleteStudentById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Student> GetAllStudents()
        {
            throw new NotImplementedException();
        }

        public Student GetStudentById(int id)
        {
            throw new NotImplementedException();
        }

        public Status UpdateStudent(int id, Student student)
        {
            throw new NotImplementedException();
        }
    }
}
