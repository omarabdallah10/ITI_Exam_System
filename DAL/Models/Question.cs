using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Question
{
    public int QId { get; set; }

    public string? QText { get; set; }

    public string? Type { get; set; }

    public int? Answer { get; set; }

    public int? Score { get; set; }

    public int? CrsId { get; set; }

    public virtual Course? Crs { get; set; }

    public virtual ICollection<StdExam> StdExams { get; set; } = new List<StdExam>();

    public virtual ICollection<Choice> Ches { get; set; } = new List<Choice>();

    public virtual ICollection<Exam> Exes { get; set; } = new List<Exam>();
}
