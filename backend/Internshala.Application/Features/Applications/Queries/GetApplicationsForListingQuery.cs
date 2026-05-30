using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Applications.DTOs;
using Internshala.Shared.Constants;
using Internshala.Shared.Exceptions;
using Internshala.Shared.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Applications.Queries;

public sealed record GetApplicationsForListingQuery(
    EmployerApplicationFilterParams Filters) : IRequest<PagedResult<EmployerApplicationListDto>>;

public sealed class GetApplicationsForListingQueryHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<GetApplicationsForListingQuery, PagedResult<EmployerApplicationListDto>>
{
    public async Task<PagedResult<EmployerApplicationListDto>> Handle(
        GetApplicationsForListingQuery request,
        CancellationToken cancellationToken)
    {
        if (currentUser.Role != RoleConstants.Employer && currentUser.Role != RoleConstants.Admin)
            throw new ForbiddenException("Only employers can view listing applications.");

        var userId = currentUser.UserId
            ?? throw new UnauthorizedException();

        var f = request.Filters;

        if (currentUser.Role == RoleConstants.Employer)
        {
            var employer = await db.Employers
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.UserId == userId, cancellationToken)
                ?? throw new ForbiddenException("Employer profile not found.");

            var ownsListing = f.ListingType == "Internship"
                ? await db.Internships.AnyAsync(i => i.Id == f.ListingId && i.EmployerId == employer.Id, cancellationToken)
                : await db.Jobs.AnyAsync(j => j.Id == f.ListingId && j.EmployerId == employer.Id, cancellationToken);

            if (!ownsListing)
                throw new ForbiddenException("You can only view applications for your own listings.");
        }

        var query = db.Applications
            .AsNoTracking()
            .Where(a => a.ListingType == f.ListingType
                && ((f.ListingType == "Internship" && a.InternshipId == f.ListingId)
                    || (f.ListingType == "Job" && a.JobId == f.ListingId)));

        if (f.Status.HasValue)
            query = query.Where(a => a.Status == f.Status);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((f.Page - 1) * f.PageSize)
            .Take(f.PageSize)
            .Select(a => new EmployerApplicationListDto(
                a.Id,
                a.StudentId,
                a.Student.User.FullName,
                a.Student.User.Email,
                a.Student.CurrentInstitution,
                a.Student.CourseOfStudy,
                a.Student.GraduationYear,
                a.Status,
                a.CoverLetter,
                a.ResumeUrl,
                a.AvailabilityDate,
                a.ExpectedStipend,
                a.EmployerNote,
                a.CreatedAt))
            .ToListAsync(cancellationToken);

        return PagedResult<EmployerApplicationListDto>.Create(items, f.Page, f.PageSize, totalCount);
    }
}
