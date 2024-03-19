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

        public User GetStudentById(int id)
        {
            var std = context.Users.FirstOrDefault(s => s.UId == id);
            return std;
        }

        public int GetStudentByIdByName(string? userName)
        {
            var std = context.Users.Include(s => s.Student).FirstOrDefault(s => s.Username == userName);
            return std.UId;
        }

        //public Status CreateStudent(Student student)
        //{
        //    throw new NotImplementedException();
        //}

        //public Status UpdateStudent(int id, Student student)
        //{
        //    throw new NotImplementedException();
        //}

        //public Status DeleteStudentById(int id)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
