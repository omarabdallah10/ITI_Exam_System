using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Instructor
{
    public int InsId { get; set; }

    public DateOnly? HireDate { get; set; }

    public int? DeptId { get; set; }

    public virtual Department? Dept { get; set; }

    public virtual User Ins { get; set; } = null!;

    public virtual ICollection<Course> Crs { get; set; } = new List<Course>();
}
