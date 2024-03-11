using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Topic
{
    public int CrsId { get; set; }

    public string? TopicName { get; set; }

    public virtual Course Crs { get; set; } = null!;
}
