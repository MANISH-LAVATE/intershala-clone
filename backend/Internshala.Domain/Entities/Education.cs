using Internshala.Domain.Common;

namespace Internshala.Domain.Entities;

public class Education : AuditableEntity
{
    public int StudentId { get; set; }
    public string Institution { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string FieldOfStudy { get; set; } = string.Empty;
    public short StartYear { get; set; }
    public short? EndYear { get; set; }
    public bool IsCurrent { get; set; }
    public decimal? Grade { get; set; }
    public Student Student { get; set; } = null!;
}
