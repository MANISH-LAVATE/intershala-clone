using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Search.DTOs;
using Internshala.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Search.Queries;

public sealed record GlobalSearchQuery(string Query, int Limit = 15) : IRequest<GlobalSearchResult>;

public sealed class GlobalSearchQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GlobalSearchQuery, GlobalSearchResult>
{
    public async Task<GlobalSearchResult> Handle(
        GlobalSearchQuery request,
        CancellationToken cancellationToken)
    {
        var q = request.Query.Trim();
        var limit = Math.Clamp(request.Limit, 1, 50);

        if (string.IsNullOrWhiteSpace(q))
            return new GlobalSearchResult([], 0, q);

        var perEntity = Math.Max(1, limit / 3);

        var internships = await db.Internships
            .AsNoTracking()
            .Where(i => i.IsActive && i.Status == InternshipStatus.Active &&
                (i.Title.Contains(q) || i.Employer.CompanyName.Contains(q)))
            .OrderByDescending(i => i.CreatedAt)
            .Take(perEntity)
            .Select(i => new SearchResultDto(
                "Internship",
                i.Id,
                i.Title,
                i.Employer.CompanyName,
                i.InternshipType.ToString(),
                $"/internships/{i.Id}"))
            .ToListAsync(cancellationToken);

        var jobs = await db.Jobs
            .AsNoTracking()
            .Where(j => j.IsActive && (j.Title.Contains(q) || j.Employer.CompanyName.Contains(q)))
            .OrderByDescending(j => j.CreatedAt)
            .Take(perEntity)
            .Select(j => new SearchResultDto(
                "Job",
                j.Id,
                j.Title,
                j.Employer.CompanyName,
                j.JobType.ToString(),
                $"/jobs/{j.Id}"))
            .ToListAsync(cancellationToken);

        var courses = await db.Courses
            .AsNoTracking()
            .Where(c => c.IsPublished && (c.Title.Contains(q) || (c.Instructor != null && c.Instructor.Contains(q))))
            .OrderByDescending(c => c.EnrolledCount)
            .Take(perEntity)
            .Select(c => new SearchResultDto(
                "Course",
                c.Id,
                c.Title,
                c.Instructor,
                c.Level.ToString(),
                $"/courses/{c.Id}"))
            .ToListAsync(cancellationToken);

        var all = internships.Concat(jobs).Concat(courses).ToList();

        return new GlobalSearchResult(all, all.Count, q);
    }
}
