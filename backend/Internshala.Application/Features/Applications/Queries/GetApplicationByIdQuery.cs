using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Applications.DTOs;
using Internshala.Domain.Enums;
using Internshala.Shared.Constants;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Applications.Queries;

public sealed record GetApplicationByIdQuery(int ApplicationId) : IRequest<ApplicationDetailDto>;

public sealed class GetApplicationByIdQueryHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<GetApplicationByIdQuery, ApplicationDetailDto>
{
    public async Task<ApplicationDetailDto> Handle(
        GetApplicationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? throw new UnauthorizedException();

        var application = await db.Applications
            .AsNoTracking()
            .Include(a => a.Internship).ThenInclude(i => i!.Employer)
            .Include(a => a.Job).ThenInclude(j => j!.Employer)
            .Include(a => a.Student).ThenInclude(s => s.User)
            .Include(a => a.StatusHistory)
            .FirstOrDefaultAsync(a => a.Id == request.ApplicationId, cancellationToken)
            ?? throw new NotFoundException("Application", request.ApplicationId);

        // Students can only see their own applications
        if (currentUser.Role == RoleConstants.Student)
        {
            var student = await db.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

            if (student is null || application.StudentId != student.Id)
                throw new ForbiddenException("You can only view your own applications.");
        }
        else if (currentUser.Role == RoleConstants.Employer)
        {
            var employer = await db.Employers
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.UserId == userId, cancellationToken)
                ?? throw new ForbiddenException();

            var ownsListing = application.ListingType == "Internship"
                ? application.Internship?.EmployerId == employer.Id
                : application.Job?.EmployerId == employer.Id;

            if (!ownsListing)
                throw new ForbiddenException("You can only view applications for your own listings.");
        }

        var history = application.StatusHistory
            .OrderBy(h => h.CreatedAt)
            .Select(h => new StatusHistoryDto(h.FromStatus, h.ToStatus, h.Comment, h.CreatedAt))
            .ToList();

        return new ApplicationDetailDto(
            application.Id,
            application.ListingType,
            application.InternshipId,
            application.Internship?.Title,
            application.Internship?.Description,
            application.JobId,
            application.Job?.Title,
            application.Job?.Description,
            application.Internship?.EmployerId ?? application.Job?.EmployerId ?? 0,
            application.Internship?.Employer.CompanyName ?? application.Job?.Employer.CompanyName ?? string.Empty,
            application.Internship?.Employer.CompanyLogoUrl ?? application.Job?.Employer.CompanyLogoUrl,
            application.Status,
            application.CoverLetter,
            application.ResumeUrl,
            application.AvailabilityDate,
            application.ExpectedStipend,
            application.EmployerNote,
            application.RejectionReason,
            application.CreatedAt,
            application.WithdrawnAt,
            history);
    }
}
