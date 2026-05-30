using Internshala.Domain.Common;
using Internshala.Domain.Enums;

namespace Internshala.Domain.Entities;

public class Course : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Instructor { get; set; }
    public string? ThumbnailUrl { get; set; }
    public CourseLevel Level { get; set; } = CourseLevel.Beginner;
    public int CategoryId { get; set; }
    public int DurationHours { get; set; }
    public bool IsFree { get; set; } = true;
    public decimal Price { get; set; }
    public bool IsPublished { get; set; }
    public int EnrolledCount { get; set; }
    public string? Language { get; set; } = "English";
    public string? Prerequisites { get; set; }
    public string? WhatYouLearn { get; set; }

    public Category Category { get; set; } = null!;
    public ICollection<CourseModule> Modules { get; set; } = [];
    public ICollection<Enrollment> Enrollments { get; set; } = [];
}
