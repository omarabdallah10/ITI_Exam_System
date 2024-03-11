using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class StdExam
{
    public int StdId { get; set; }

    public int ExId { get; set; }

    public int QId { get; set; }

    public int? StdAnswer { get; set; }

    public virtual Exam Ex { get; set; } = null!;

    public virtual Question QIdNavigation { get; set; } = null!;

    public virtual Student Std { get; set; } = null!;
}
