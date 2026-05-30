using Internshala.Domain.Enums;

namespace Internshala.Application.Features.Courses.DTOs;

public sealed record CourseListDto(
    int Id,
    string Title,
    string? Instructor,
    string? ThumbnailUrl,
    CourseLevel Level,
    string CategoryName,
    int DurationHours,
    bool IsFree,
    decimal Price,
    int EnrolledCount,
    int ModuleCount,
    DateTime CreatedAt);

public sealed record CourseDetailDto(
    int Id,
    string Title,
    string Description,
    string? Instructor,
    string? ThumbnailUrl,
    CourseLevel Level,
    int CategoryId,
    string CategoryName,
    int DurationHours,
    bool IsFree,
    decimal Price,
    int EnrolledCount,
    string? Language,
    string? Prerequisites,
    string? WhatYouLearn,
    IReadOnlyList<CourseModuleDto> Modules,
    bool IsEnrolled,
    byte EnrollmentProgress);

public sealed record CourseModuleDto(
    int Id,
    string Title,
    string? Description,
    string? VideoUrl,
    int OrderIndex,
    int DurationMinutes,
    bool IsPreview);

public sealed record EnrollmentDto(
    int Id,
    int CourseId,
    string CourseTitle,
    string? CourseThumbnailUrl,
    byte ProgressPercent,
    int CompletedModules,
    int TotalModules,
    DateTime EnrolledAt,
    DateTime? CompletedAt,
    DateTime? LastAccessedAt);

public sealed record CourseFilterParams
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? Search { get; init; }
    public int? CategoryId { get; init; }
    public string? Level { get; init; }
    public bool? IsFree { get; init; }
    public string SortBy { get; init; } = "createdAt";
}
