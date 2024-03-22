using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public partial class Course
{
    public int CrsId { get; set; }

    [Required(ErrorMessage = "Course Name is required")]
    [StringLength(20, ErrorMessage = "Course name can't exceed then 20 characters")]
    public string CrsName { get; set; }

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

    public virtual ICollection<Instructor> Ins { get; set; } = new List<Instructor>();
}
