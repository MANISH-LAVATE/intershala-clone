namespace Internshala.Application.Features.Profile.DTOs;

public sealed record StudentProfileDto(
    int Id,
    int UserId,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string? ProfilePictureUrl,
    DateOnly? DateOfBirth,
    string? Gender,
    string? CurrentInstitution,
    string? CourseOfStudy,
    short? GraduationYear,
    decimal? Gpa,
    string? Bio,
    string? LinkedInUrl,
    string? GitHubUrl,
    string? PortfolioUrl,
    byte ProfileCompleteness,
    bool IsProfileComplete,
    IReadOnlyList<EducationDto> Educations,
    IReadOnlyList<ExperienceDto> Experiences,
    ResumeDto? Resume);

public sealed record EducationDto(
    int Id,
    string Institution,
    string Degree,
    string FieldOfStudy,
    short StartYear,
    short? EndYear,
    bool IsCurrent,
    decimal? Grade);

public sealed record ExperienceDto(
    int Id,
    string Title,
    string Company,
    string? Description,
    DateOnly StartDate,
    DateOnly? EndDate,
    bool IsCurrent);

public sealed record ResumeDto(
    int Id,
    string? FileUrl,
    string? FileName,
    long? FileSizeBytes,
    DateTime? UploadedAt);

public sealed record UpdateStudentProfileRequest(
    string FirstName,
    string LastName,
    string? PhoneNumber,
    DateOnly? DateOfBirth,
    string? Gender,
    string? CurrentInstitution,
    string? CourseOfStudy,
    short? GraduationYear,
    decimal? Gpa,
    string? Bio,
    string? LinkedInUrl,
    string? GitHubUrl,
    string? PortfolioUrl);

public sealed record AddEducationRequest(
    string Institution,
    string Degree,
    string FieldOfStudy,
    short StartYear,
    short? EndYear,
    bool IsCurrent,
    decimal? Grade);

public sealed record UpdateEducationRequest(
    string Institution,
    string Degree,
    string FieldOfStudy,
    short StartYear,
    short? EndYear,
    bool IsCurrent,
    decimal? Grade);

public sealed record AddExperienceRequest(
    string Title,
    string Company,
    string? Description,
    DateOnly StartDate,
    DateOnly? EndDate,
    bool IsCurrent);

public sealed record UpdateExperienceRequest(
    string Title,
    string Company,
    string? Description,
    DateOnly StartDate,
    DateOnly? EndDate,
    bool IsCurrent);
