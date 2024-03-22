using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Exam
{
    public int ExId { get; set; }

    public TimeOnly? Duration { get; set; }

    public DateOnly? Date { get; set; }

    public int? TotalScore { get; set; }

    public int? CrsId { get; set; }

    public virtual Course? Crs { get; set; }

    public virtual ICollection<StdExam> StdExams { get; set; } = new List<StdExam>();

    public virtual ICollection<Question> QIds { get; set; } = new List<Question>();

    public override string ToString()
    {
        return $"{Crs}, {Duration}, {Date}";
    }
}
