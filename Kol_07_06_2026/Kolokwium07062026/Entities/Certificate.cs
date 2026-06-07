using System;
using System.Collections.Generic;

namespace Kolokwium07062026.Entities;

public partial class Certificate
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CourseId { get; set; }

    public DateTime IssueDate { get; set; }

    public Guid CertificateCode { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
