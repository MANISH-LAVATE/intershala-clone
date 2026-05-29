using Internshala.Domain.Common;
using Internshala.Domain.Enums;

namespace Internshala.Domain.Entities;

public class Internship : AuditableEntity
{
    public int EmployerId { get; set; }
    public int CategoryId { get; set; }
    public int? LocationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Responsibilities { get; set; }
    public string? Requirements { get; set; }
    public InternshipType InternshipType { get; set; }
    public int? StipendMin { get; set; }
    public int? StipendMax { get; set; }
    public bool IsPaid { get; set; } = true;
    public byte DurationMonths { get; set; }
    public string StartDateType { get; set; } = "Immediate";
    public DateOnly? StartDate { get; set; }
    public short OpeningsCount { get; set; } = 1;
    public DateOnly? ApplicationDeadline { get; set; }
    public InternshipStatus Status { get; set; } = InternshipStatus.Draft;
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public DateOnly? FeaturedUntil { get; set; }
    public int ViewsCount { get; set; }
    public int ApplicationsCount { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }

    public Employer Employer { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public Location? Location { get; set; }
    public ICollection<Application> Applications { get; set; } = [];
    public ICollection<Skill> Skills { get; set; } = [];
}
