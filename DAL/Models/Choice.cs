using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Choice
{
    public int ChId { get; set; }

    public string? Text { get; set; }

    public virtual ICollection<Question> QIds { get; set; } = new List<Question>();
}
