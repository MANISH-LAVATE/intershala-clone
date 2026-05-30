using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Applications.DTOs;
using Internshala.Domain.Enums;
using Internshala.Shared.Constants;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DomainApplication = Internshala.Domain.Entities.Application;

namespace Internshala.Application.Features.Applications.Commands;

public sealed record ApplyCommand(ApplyRequest Request) : IRequest<int>;

public sealed class ApplyCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<ApplyCommand, int>
{
    public async Task<int> Handle(ApplyCommand command, CancellationToken cancellationToken)
    {
        if (currentUser.Role != RoleConstants.Student)
            throw new ForbiddenException("Only students can apply to listings.");

        var userId = currentUser.UserId
            ?? throw new UnauthorizedException();

        var student = await db.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken)
            ?? throw new NotFoundException("Student profile", userId);

        var req = command.Request;
        int? internshipId = null;
        int? jobId = null;

        if (req.ListingType == "Internship")
        {
            var internship = await db.Internships
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == req.ListingId && i.IsActive, cancellationToken)
                ?? throw new NotFoundException("Internship", req.ListingId);

            internshipId = internship.Id;
        }
        else
        {
            var job = await db.Jobs
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Id == req.ListingId && j.IsActive, cancellationToken)
                ?? throw new NotFoundException("Job", req.ListingId);

            jobId = job.Id;
        }

        var alreadyApplied = await db.Applications.AnyAsync(
            a => a.StudentId == student.Id
              && a.ListingType == req.ListingType
              && ((req.ListingType == "Internship" && a.InternshipId == internshipId)
                  || (req.ListingType == "Job" && a.JobId == jobId))
              && a.Status != ApplicationStatus.Withdrawn,
            cancellationToken);

        if (alreadyApplied)
            throw new ConflictException("You have already applied to this listing.");

        var application = new DomainApplication
        {
            StudentId = student.Id,
            ListingType = req.ListingType,
            InternshipId = internshipId,
            JobId = jobId,
            Status = ApplicationStatus.Applied,
            CoverLetter = req.CoverLetter,
            ResumeUrl = req.ResumeUrl,
            AvailabilityDate = req.AvailabilityDate,
            ExpectedStipend = req.ExpectedStipend
        };

        db.Applications.Add(application);

        if (internshipId.HasValue)
        {
            var internship = await db.Internships.FindAsync([internshipId.Value], cancellationToken);
            if (internship is not null)
                internship.ApplicationsCount++;
        }

        await db.SaveChangesAsync(cancellationToken);
        return application.Id;
    }
}
