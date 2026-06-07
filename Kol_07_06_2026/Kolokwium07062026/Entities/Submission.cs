using System;
using System.Collections.Generic;

namespace Kolokwium07062026.Entities;

public partial class Submission
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int AssignmentId { get; set; }

    public DateTime SubmittedAt { get; set; }

    public int Score { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
