using Internshala.Domain.Common;

namespace Internshala.Domain.Entities;

public class Enrollment : AuditableEntity
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public byte ProgressPercent { get; set; }
    public int CompletedModules { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? LastAccessedAt { get; set; }

    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
}
