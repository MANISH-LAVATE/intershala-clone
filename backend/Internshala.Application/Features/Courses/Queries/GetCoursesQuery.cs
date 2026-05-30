using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Courses.DTOs;
using Internshala.Domain.Enums;
using Internshala.Shared.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Courses.Queries;

public sealed record GetCoursesQuery(CourseFilterParams Filters) : IRequest<PagedResult<CourseListDto>>;

public sealed class GetCoursesQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetCoursesQuery, PagedResult<CourseListDto>>
{
    public async Task<PagedResult<CourseListDto>> Handle(
        GetCoursesQuery request,
        CancellationToken cancellationToken)
    {
        var f = request.Filters;

        var query = db.Courses
            .AsNoTracking()
            .Where(c => c.IsPublished);

        if (!string.IsNullOrWhiteSpace(f.Search))
            query = query.Where(c => c.Title.Contains(f.Search) ||
                (c.Instructor != null && c.Instructor.Contains(f.Search)));

        if (f.CategoryId.HasValue)
            query = query.Where(c => c.CategoryId == f.CategoryId);

        if (!string.IsNullOrWhiteSpace(f.Level) && Enum.TryParse<CourseLevel>(f.Level, out var level))
            query = query.Where(c => c.Level == level);

        if (f.IsFree.HasValue)
            query = query.Where(c => c.IsFree == f.IsFree);

        var totalCount = await query.CountAsync(cancellationToken);

        query = f.SortBy.ToLowerInvariant() switch
        {
            "popular" => query.OrderByDescending(c => c.EnrolledCount),
            _ => query.OrderByDescending(c => c.CreatedAt)
        };

        var items = await query
            .Skip((f.Page - 1) * f.PageSize)
            .Take(f.PageSize)
            .Select(c => new CourseListDto(
                c.Id,
                c.Title,
                c.Instructor,
                c.ThumbnailUrl,
                c.Level,
                c.Category.Name,
                c.DurationHours,
                c.IsFree,
                c.Price,
                c.EnrolledCount,
                c.Modules.Count(m => !m.IsDeleted),
                c.CreatedAt))
            .ToListAsync(cancellationToken);

        return PagedResult<CourseListDto>.Create(items, f.Page, f.PageSize, totalCount);
    }
}
