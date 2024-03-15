using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IRepository
{
    public interface IExamRepository
    {
        List<Exam> GetAllExamByStudentId(int StudentId);
        Exam GetCurrentExamByStudentId(int StudentId);

    }
}
