using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModels
{
    public class StudentExamViewModel
    {
        public int StdId { get; set; }
        public Exam Exam{ get; set; }
        public int ExamId{ get; set; }
        public Dictionary<int, int?> QuestionsAnswers{ get; set; }
        public List<Question> Questions{ get; set; }

    }
}
