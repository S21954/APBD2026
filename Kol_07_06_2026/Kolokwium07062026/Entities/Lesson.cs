using System;
using System.Collections.Generic;

namespace Kolokwium07062026.Entities;

public partial class Lesson
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public int DurationMinutes { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual Course Course { get; set; } = null!;
}
