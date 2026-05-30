using Internshala.Domain.Enums;

namespace Internshala.Application.Features.Internships.DTOs;

public sealed record InternshipListDto(
    int Id,
    string Title,
    string CompanyName,
    string? CompanyLogoUrl,
    bool IsCompanyVerified,
    string? Location,
    InternshipType InternshipType,
    int? StipendMin,
    int? StipendMax,
    bool IsPaid,
    byte DurationMonths,
    DateOnly? ApplicationDeadline,
    int ApplicationsCount,
    bool IsFeatured,
    DateTime CreatedAt,
    IReadOnlyList<string> Skills);

public sealed record InternshipDetailDto(
    int Id,
    string Title,
    string Description,
    string? Responsibilities,
    string? Requirements,
    int EmployerId,
    string CompanyName,
    string? CompanyLogoUrl,
    string? CompanyWebsite,
    bool IsCompanyVerified,
    string? CompanyDescription,
    int CategoryId,
    string CategoryName,
    string? Location,
    InternshipType InternshipType,
    int? StipendMin,
    int? StipendMax,
    bool IsPaid,
    byte DurationMonths,
    string StartDateType,
    DateOnly? StartDate,
    short OpeningsCount,
    DateOnly? ApplicationDeadline,
    int ApplicationsCount,
    int ViewsCount,
    bool IsFeatured,
    DateTime PublishedAt,
    IReadOnlyList<string> Skills);

public sealed record InternshipFilterParams
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? Search { get; init; }
    public int? CategoryId { get; init; }
    public int? LocationId { get; init; }
    public int? StipendMin { get; init; }
    public bool? IsRemote { get; init; }
    public string? InternshipType { get; init; }
    public int? DurationMonths { get; init; }
    public string SortBy { get; init; } = "createdAt";
    public string SortOrder { get; init; } = "desc";
}

public sealed record CreateInternshipRequest(
    string Title,
    string Description,
    string? Responsibilities,
    string? Requirements,
    int CategoryId,
    int? LocationId,
    string InternshipType,
    int? StipendMin,
    int? StipendMax,
    bool IsPaid,
    byte DurationMonths,
    string StartDateType,
    DateOnly? StartDate,
    short OpeningsCount,
    DateOnly? ApplicationDeadline,
    IReadOnlyList<int> SkillIds);

public sealed record UpdateInternshipRequest(
    string Title,
    string Description,
    string? Responsibilities,
    string? Requirements,
    int CategoryId,
    int? LocationId,
    string InternshipType,
    int? StipendMin,
    int? StipendMax,
    bool IsPaid,
    byte DurationMonths,
    string StartDateType,
    DateOnly? StartDate,
    short OpeningsCount,
    DateOnly? ApplicationDeadline,
    IReadOnlyList<int> SkillIds);
