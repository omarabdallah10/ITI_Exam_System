using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public partial class User
{
    public int UId { get; set; }

    [Required(ErrorMessage = "Please enter your username")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Role is required")]
    public string Role { get; set; } = null!;

    [Required(ErrorMessage = "Enter your First name")]
    public string? Fname { get; set; }

    [Required(ErrorMessage = "Enter your Last name")]
    public string? Lname { get; set; }

    public virtual Instructor? Instructor { get; set; }

    public virtual Student? Student { get; set; }
}
