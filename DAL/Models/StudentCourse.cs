using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class StudentCourse
{
    public int SId { get; set; }

    public int CrsId { get; set; }

    public int? Grade { get; set; }

    public virtual Course Crs { get; set; } = null!;

    public virtual Student SIdNavigation { get; set; } = null!;
}
