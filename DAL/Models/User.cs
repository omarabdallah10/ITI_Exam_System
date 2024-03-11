using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class User
{
    public int UId { get; set; }

    public string? Username { get; set; }

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public virtual Instructor? Instructor { get; set; }

    public virtual Student? Student { get; set; }
}
