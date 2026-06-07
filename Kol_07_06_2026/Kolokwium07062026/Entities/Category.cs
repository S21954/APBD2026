using System;
using System.Collections.Generic;

namespace Kolokwium07062026.Entities;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
