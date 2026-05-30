using Internshala.Domain.Enums;

namespace Internshala.Application.Features.Applications.DTOs;

public sealed record ApplicationListDto(
    int Id,
    string ListingType,
    int? InternshipId,
    string? InternshipTitle,
    int? JobId,
    string? JobTitle,
    string CompanyName,
    string? CompanyLogoUrl,
    ApplicationStatus Status,
    string? CoverLetter,
    string ResumeUrl,
    DateOnly? AvailabilityDate,
    int? ExpectedStipend,
    string? EmployerNote,
    string? RejectionReason,
    DateTime AppliedAt,
    DateTime? WithdrawnAt);

public sealed record ApplicationDetailDto(
    int Id,
    string ListingType,
    int? InternshipId,
    string? InternshipTitle,
    string? InternshipDescription,
    int? JobId,
    string? JobTitle,
    string? JobDescription,
    int EmployerId,
    string CompanyName,
    string? CompanyLogoUrl,
    ApplicationStatus Status,
    string? CoverLetter,
    string ResumeUrl,
    DateOnly? AvailabilityDate,
    int? ExpectedStipend,
    string? EmployerNote,
    string? RejectionReason,
    DateTime AppliedAt,
    DateTime? WithdrawnAt,
    IReadOnlyList<StatusHistoryDto> StatusHistory);

public sealed record StatusHistoryDto(
    ApplicationStatus FromStatus,
    ApplicationStatus ToStatus,
    string? Comment,
    DateTime ChangedAt);

public sealed record EmployerApplicationListDto(
    int Id,
    int StudentId,
    string StudentFullName,
    string? StudentEmail,
    string? StudentInstitution,
    string? StudentCourse,
    short? GraduationYear,
    ApplicationStatus Status,
    string? CoverLetter,
    string ResumeUrl,
    DateOnly? AvailabilityDate,
    int? ExpectedStipend,
    string? EmployerNote,
    DateTime AppliedAt);

public sealed record ApplyRequest(
    string ListingType,
    int ListingId,
    string? CoverLetter,
    string ResumeUrl,
    DateOnly? AvailabilityDate,
    int? ExpectedStipend);

public sealed record UpdateApplicationStatusRequest(
    ApplicationStatus NewStatus,
    string? Note);

public sealed record ApplicationFilterParams
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public ApplicationStatus? Status { get; init; }
    public string? ListingType { get; init; }
}

public sealed record EmployerApplicationFilterParams
{
    public int ListingId { get; init; }
    public string ListingType { get; init; } = "Internship";
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public ApplicationStatus? Status { get; init; }
}
