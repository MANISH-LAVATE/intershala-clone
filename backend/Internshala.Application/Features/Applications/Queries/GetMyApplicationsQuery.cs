using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Applications.DTOs;
using Internshala.Shared.Exceptions;
using Internshala.Shared.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Applications.Queries;

public sealed record GetMyApplicationsQuery(ApplicationFilterParams Filters) : IRequest<PagedResult<ApplicationListDto>>;

public sealed class GetMyApplicationsQueryHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<GetMyApplicationsQuery, PagedResult<ApplicationListDto>>
{
    public async Task<PagedResult<ApplicationListDto>> Handle(
        GetMyApplicationsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? throw new UnauthorizedException();

        var student = await db.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken)
            ?? throw new NotFoundException("Student profile", userId);

        var f = request.Filters;

        var query = db.Applications
            .AsNoTracking()
            .Where(a => a.StudentId == student.Id);

        if (f.Status.HasValue)
            query = query.Where(a => a.Status == f.Status);

        if (!string.IsNullOrWhiteSpace(f.ListingType))
            query = query.Where(a => a.ListingType == f.ListingType);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((f.Page - 1) * f.PageSize)
            .Take(f.PageSize)
            .Select(a => new ApplicationListDto(
                a.Id,
                a.ListingType,
                a.InternshipId,
                a.Internship != null ? a.Internship.Title : null,
                a.JobId,
                a.Job != null ? a.Job.Title : null,
                a.Internship != null
                    ? a.Internship.Employer.CompanyName
                    : a.Job != null ? a.Job.Employer.CompanyName : string.Empty,
                a.Internship != null
                    ? a.Internship.Employer.CompanyLogoUrl
                    : a.Job != null ? a.Job.Employer.CompanyLogoUrl : null,
                a.Status,
                a.CoverLetter,
                a.ResumeUrl,
                a.AvailabilityDate,
                a.ExpectedStipend,
                a.EmployerNote,
                a.RejectionReason,
                a.CreatedAt,
                a.WithdrawnAt))
            .ToListAsync(cancellationToken);

        return PagedResult<ApplicationListDto>.Create(items, f.Page, f.PageSize, totalCount);
    }
}
