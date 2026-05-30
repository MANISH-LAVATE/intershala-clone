using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Admin.DTOs;
using Internshala.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Admin.Queries;

public sealed record GetAdminAnalyticsQuery : IRequest<AdminAnalyticsDto>;

public sealed class GetAdminAnalyticsQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetAdminAnalyticsQuery, AdminAnalyticsDto>
{
    public async Task<AdminAnalyticsDto> Handle(
        GetAdminAnalyticsQuery request,
        CancellationToken cancellationToken)
    {
        var totalUsers = await db.Users.CountAsync(cancellationToken);
        var totalStudents = await db.Students.CountAsync(cancellationToken);
        var totalEmployers = await db.Employers.CountAsync(cancellationToken);
        var totalInternships = await db.Internships.CountAsync(cancellationToken);
        var activeInternships = await db.Internships.CountAsync(i => i.IsActive, cancellationToken);
        var totalJobs = await db.Jobs.CountAsync(cancellationToken);
        var activeJobs = await db.Jobs.CountAsync(j => j.IsActive, cancellationToken);
        var totalApplications = await db.Applications.CountAsync(cancellationToken);
        var totalCourses = await db.Courses.CountAsync(cancellationToken);
        var totalEnrollments = await db.Enrollments.CountAsync(cancellationToken);

        var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
        var monthlyApps = await db.Applications
            .AsNoTracking()
            .Where(a => a.CreatedAt >= sixMonthsAgo)
            .GroupBy(a => new { a.CreatedAt.Year, a.CreatedAt.Month })
            .Select(g => new MonthlyStatDto(
                $"{g.Key.Year}-{g.Key.Month:D2}",
                g.Count()))
            .OrderBy(m => m.Month)
            .ToListAsync(cancellationToken);

        return new AdminAnalyticsDto(
            totalUsers, totalStudents, totalEmployers,
            totalInternships, activeInternships,
            totalJobs, activeJobs,
            totalApplications, totalCourses, totalEnrollments,
            monthlyApps);
    }
}
