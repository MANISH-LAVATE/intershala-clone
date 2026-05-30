using Internshala.Domain.Enums;

namespace Internshala.Application.Features.Admin.DTOs;

public sealed record AdminUserDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    UserRole Role,
    bool IsActive,
    bool IsEmailVerified,
    DateTime CreatedAt,
    DateTime? LastLoginAt);

public sealed record AdminAnalyticsDto(
    int TotalUsers,
    int TotalStudents,
    int TotalEmployers,
    int TotalInternships,
    int ActiveInternships,
    int TotalJobs,
    int ActiveJobs,
    int TotalApplications,
    int TotalCourses,
    int TotalEnrollments,
    IReadOnlyList<MonthlyStatDto> MonthlyApplications);

public sealed record MonthlyStatDto(string Month, int Count);

public sealed record AdminUserFilterParams
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? Search { get; init; }
    public string? Role { get; init; }
    public bool? IsActive { get; init; }
}
