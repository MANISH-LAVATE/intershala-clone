using Internshala.Domain.Common;
using Internshala.Domain.Enums;

namespace Internshala.Domain.Entities;

public class Application : AuditableEntity
{
    public int StudentId { get; set; }
    public string ListingType { get; set; } = "Internship";
    public int? InternshipId { get; set; }
    public int? JobId { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;
    public string? CoverLetter { get; set; }
    public string ResumeUrl { get; set; } = string.Empty;
    public DateOnly? AvailabilityDate { get; set; }
    public int? ExpectedStipend { get; set; }
    public string? EmployerNote { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime? WithdrawnAt { get; set; }

    public Student Student { get; set; } = null!;
    public Internship? Internship { get; set; }
    public Job? Job { get; set; }
    public ICollection<ApplicationStatusHistory> StatusHistory { get; set; } = [];
}
