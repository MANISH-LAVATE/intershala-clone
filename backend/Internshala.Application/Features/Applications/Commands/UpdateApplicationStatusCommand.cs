using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Applications.DTOs;
using Internshala.Domain.Enums;
using Internshala.Domain.Services;
using Internshala.Shared.Constants;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Applications.Commands;

public sealed record UpdateApplicationStatusCommand(
    int ApplicationId,
    UpdateApplicationStatusRequest Request) : IRequest;

public sealed class UpdateApplicationStatusCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<UpdateApplicationStatusCommand>
{
    public async Task Handle(UpdateApplicationStatusCommand command, CancellationToken cancellationToken)
    {
        if (currentUser.Role != RoleConstants.Employer && currentUser.Role != RoleConstants.Admin)
            throw new ForbiddenException("Only employers can update application status.");

        var userId = currentUser.UserId
            ?? throw new UnauthorizedException();

        var application = await db.Applications
            .FirstOrDefaultAsync(a => a.Id == command.ApplicationId, cancellationToken)
            ?? throw new NotFoundException("Application", command.ApplicationId);

        if (currentUser.Role == RoleConstants.Employer)
        {
            var employer = await db.Employers
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.UserId == userId, cancellationToken)
                ?? throw new ForbiddenException("Employer profile not found.");

            var ownsListing = application.ListingType == "Internship"
                ? await db.Internships.AnyAsync(i => i.Id == application.InternshipId && i.EmployerId == employer.Id, cancellationToken)
                : await db.Jobs.AnyAsync(j => j.Id == application.JobId && j.EmployerId == employer.Id, cancellationToken);

            if (!ownsListing)
                throw new ForbiddenException("You can only manage applications for your own listings.");
        }

        var newStatus = command.Request.NewStatus;

        if (!ApplicationStateMachine.CanTransition(application.Status, newStatus))
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["Status"] = [$"Transition from '{application.Status}' to '{newStatus}' is not allowed."]
            });

        var fromStatus = application.Status;
        application.Status = newStatus;

        if (newStatus == ApplicationStatus.Rejected && command.Request.Note is not null)
            application.RejectionReason = command.Request.Note;
        else if (command.Request.Note is not null)
            application.EmployerNote = command.Request.Note;

        db.ApplicationStatusHistories.Add(new()
        {
            ApplicationId = application.Id,
            FromStatus = fromStatus,
            ToStatus = newStatus,
            Comment = command.Request.Note,
            ChangedBy = userId
        });

        await db.SaveChangesAsync(cancellationToken);
    }
}
