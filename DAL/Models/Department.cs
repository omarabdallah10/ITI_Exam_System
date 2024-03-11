using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Department
{
    public int DeptId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();
}
