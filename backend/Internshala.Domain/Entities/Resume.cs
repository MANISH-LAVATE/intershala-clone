using Internshala.Domain.Common;

namespace Internshala.Domain.Entities;

public class Resume : AuditableEntity
{
    public int StudentId { get; set; }
    public string? FileUrl { get; set; }
    public string? FileName { get; set; }
    public long? FileSizeBytes { get; set; }
    public DateTime? UploadedAt { get; set; }
    public Student Student { get; set; } = null!;
}
