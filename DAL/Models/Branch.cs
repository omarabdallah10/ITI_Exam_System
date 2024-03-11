using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Branch
{
    public int BranchId { get; set; }

    public string? Name { get; set; }

    public string? Location { get; set; }

    public virtual ICollection<Department> Depts { get; set; } = new List<Department>();
}
