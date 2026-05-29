using Internshala.Domain.Common;

namespace Internshala.Domain.Entities;

public class Skill : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public ICollection<Internship> Internships { get; set; } = [];
    public ICollection<Job> Jobs { get; set; } = [];
}
