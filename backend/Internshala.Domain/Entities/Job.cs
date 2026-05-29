using Internshala.Domain.Common;
using Internshala.Domain.Enums;

namespace Internshala.Domain.Entities;

public class Job : AuditableEntity
{
    public int EmployerId { get; set; }
    public int CategoryId { get; set; }
    public int? LocationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Requirements { get; set; }
    public InternshipType JobType { get; set; }
    public int? SalaryMin { get; set; }
    public int? SalaryMax { get; set; }
    public int? ExperienceYearsMin { get; set; }
    public DateOnly? ApplicationDeadline { get; set; }
    public InternshipStatus Status { get; set; } = InternshipStatus.Draft;
    public bool IsActive { get; set; }
    public int ViewsCount { get; set; }
    public int ApplicationsCount { get; set; }
    public DateTime? PublishedAt { get; set; }

    public Employer Employer { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public Location? Location { get; set; }
    public ICollection<Application> Applications { get; set; } = [];
    public ICollection<Skill> Skills { get; set; } = [];
}
