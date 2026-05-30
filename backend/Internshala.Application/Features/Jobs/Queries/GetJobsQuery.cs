using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Jobs.DTOs;
using Internshala.Domain.Enums;
using Internshala.Shared.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Jobs.Queries;

public sealed record GetJobsQuery(JobFilterParams Filters) : IRequest<PagedResult<JobListDto>>;

public sealed class GetJobsQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetJobsQuery, PagedResult<JobListDto>>
{
    public async Task<PagedResult<JobListDto>> Handle(
        GetJobsQuery request,
        CancellationToken cancellationToken)
    {
        var f = request.Filters;

        var query = db.Jobs
            .AsNoTracking()
            .Where(j => j.IsActive && j.Status == InternshipStatus.Active);

        if (!string.IsNullOrWhiteSpace(f.Search))
            query = query.Where(j => j.Title.Contains(f.Search) || j.Employer.CompanyName.Contains(f.Search));

        if (f.CategoryId.HasValue) query = query.Where(j => j.CategoryId == f.CategoryId);
        if (f.IsRemote == true)
            query = query.Where(j => j.JobType == InternshipType.Remote || j.LocationId == null);
        else if (f.LocationId.HasValue)
            query = query.Where(j => j.LocationId == f.LocationId);
        if (f.SalaryMin.HasValue) query = query.Where(j => j.SalaryMin >= f.SalaryMin);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip((f.Page - 1) * f.PageSize)
            .Take(f.PageSize)
            .Select(j => new JobListDto(
                j.Id, j.Title,
                j.Employer.CompanyName, j.Employer.CompanyLogoUrl, j.Employer.IsVerified,
                j.Location != null ? j.Location.CityName : null,
                j.JobType, j.SalaryMin, j.SalaryMax, j.ExperienceYearsMin,
                j.ApplicationDeadline, j.ApplicationsCount, j.CreatedAt,
                j.Skills.Select(s => s.Name).ToList()))
            .ToListAsync(cancellationToken);

        return PagedResult<JobListDto>.Create(items, f.Page, f.PageSize, totalCount);
    }
}
