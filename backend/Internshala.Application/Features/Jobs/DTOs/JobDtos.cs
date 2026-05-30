using Internshala.Domain.Enums;

namespace Internshala.Application.Features.Jobs.DTOs;

public sealed record JobListDto(
    int Id,
    string Title,
    string CompanyName,
    string? CompanyLogoUrl,
    bool IsCompanyVerified,
    string? Location,
    InternshipType JobType,
    int? SalaryMin,
    int? SalaryMax,
    int? ExperienceYearsMin,
    DateOnly? ApplicationDeadline,
    int ApplicationsCount,
    DateTime CreatedAt,
    IReadOnlyList<string> Skills);

public sealed record JobDetailDto(
    int Id,
    string Title,
    string Description,
    string? Requirements,
    int EmployerId,
    string CompanyName,
    string? CompanyLogoUrl,
    bool IsCompanyVerified,
    int CategoryId,
    string CategoryName,
    string? Location,
    InternshipType JobType,
    int? SalaryMin,
    int? SalaryMax,
    int? ExperienceYearsMin,
    DateOnly? ApplicationDeadline,
    int ApplicationsCount,
    DateTime PublishedAt,
    IReadOnlyList<string> Skills);

public sealed record JobFilterParams
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? Search { get; init; }
    public int? CategoryId { get; init; }
    public int? LocationId { get; init; }
    public int? SalaryMin { get; init; }
    public bool? IsRemote { get; init; }
    public string SortBy { get; init; } = "createdAt";
    public string SortOrder { get; init; } = "desc";
}

public sealed record CreateJobRequest(
    string Title,
    string Description,
    string? Requirements,
    int CategoryId,
    int? LocationId,
    string JobType,
    int? SalaryMin,
    int? SalaryMax,
    int? ExperienceYearsMin,
    DateOnly? ApplicationDeadline,
    IReadOnlyList<int> SkillIds);
