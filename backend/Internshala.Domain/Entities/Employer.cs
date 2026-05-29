using Internshala.Domain.Common;

namespace Internshala.Domain.Entities;

public class Employer : AuditableEntity
{
    public int UserId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? CompanySize { get; set; }
    public int IndustryId { get; set; }
    public string? CompanyWebsite { get; set; }
    public string? CompanyLogoUrl { get; set; }
    public string? Description { get; set; }
    public short? Founded { get; set; }
    public string? HeadquartersCity { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? LinkedInUrl { get; set; }

    public User User { get; set; } = null!;
    public Category Industry { get; set; } = null!;
    public ICollection<Internship> Internships { get; set; } = [];
    public ICollection<Job> Jobs { get; set; } = [];
}
