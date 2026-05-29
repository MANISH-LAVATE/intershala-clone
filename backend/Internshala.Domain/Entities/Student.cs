using Internshala.Domain.Common;

namespace Internshala.Domain.Entities;

public class Student : AuditableEntity
{
    public int UserId { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? CurrentInstitution { get; set; }
    public string? CourseOfStudy { get; set; }
    public short? GraduationYear { get; set; }
    public decimal? Gpa { get; set; }
    public string? Bio { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? PortfolioUrl { get; set; }
    public bool IsProfileComplete { get; set; }
    public byte ProfileCompleteness { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Application> Applications { get; set; } = [];
    public ICollection<Education> Educations { get; set; } = [];
    public ICollection<Experience> Experiences { get; set; } = [];
    public Resume? Resume { get; set; }
}
