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
    public class StudentRepository : IStudentRepository
    {
        private readonly ITIContext context;

        public StudentRepository(ITIContext _context)
        {
            context = _context;
        }
        public List<Student> GetAllStudents()
        {
            return context.Students.ToList();
        }

        public Student GetStudentById(int id)
        {
            var std = context.Students.FirstOrDefault(s => s.StdId == id);
            return std;
        }

        public Status CreateStudent(Student student)
        {
            throw new NotImplementedException();
        }

        public Status UpdateStudent(int id, Student student)
        {
            throw new NotImplementedException();
        }

        public Status DeleteStudentById(int id)
        {
            throw new NotImplementedException();
        }

    }
}
