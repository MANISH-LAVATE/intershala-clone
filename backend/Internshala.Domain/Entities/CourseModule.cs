using Internshala.Domain.Common;

namespace Internshala.Domain.Entities;

public class CourseModule : AuditableEntity
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public string? Content { get; set; }
    public int OrderIndex { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsPreview { get; set; }

    public Course Course { get; set; } = null!;
}
