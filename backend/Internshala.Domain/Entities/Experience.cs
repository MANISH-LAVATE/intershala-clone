using Internshala.Domain.Common;

namespace Internshala.Domain.Entities;

public class Experience : AuditableEntity
{
    public int StudentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public Student Student { get; set; } = null!;
}
