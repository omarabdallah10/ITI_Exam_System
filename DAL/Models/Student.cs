using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Student
{
    public int StdId { get; set; }

    public int? DeptId { get; set; }

    public virtual Department? Dept { get; set; }

    public virtual User Std { get; set; } = null!;

    public virtual ICollection<StdExam> StdExams { get; set; } = new List<StdExam>();

    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
}
