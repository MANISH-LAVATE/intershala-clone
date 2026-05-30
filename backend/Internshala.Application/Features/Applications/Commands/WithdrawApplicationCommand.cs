using Internshala.Application.Common.Interfaces;
using Internshala.Domain.Enums;
using Internshala.Shared.Constants;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Applications.Commands;

public sealed record WithdrawApplicationCommand(int ApplicationId) : IRequest;

public sealed class WithdrawApplicationCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<WithdrawApplicationCommand>
{
    public async Task Handle(WithdrawApplicationCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? throw new UnauthorizedException();

        var student = await db.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken)
            ?? throw new ForbiddenException("Only students can withdraw applications.");

        var application = await db.Applications
            .FirstOrDefaultAsync(a => a.Id == command.ApplicationId && a.StudentId == student.Id, cancellationToken)
            ?? throw new NotFoundException("Application", command.ApplicationId);

        if (application.Status == ApplicationStatus.Withdrawn)
            throw new ConflictException("Application has already been withdrawn.");

        if (application.Status == ApplicationStatus.Selected || application.Status == ApplicationStatus.Rejected)
            throw new ForbiddenException("Cannot withdraw an application that has been selected or rejected.");

        var fromStatus = application.Status;
        application.Status = ApplicationStatus.Withdrawn;
        application.WithdrawnAt = DateTime.UtcNow;

        db.ApplicationStatusHistories.Add(new()
        {
            ApplicationId = application.Id,
            FromStatus = fromStatus,
            ToStatus = ApplicationStatus.Withdrawn,
            Comment = "Withdrawn by student",
            ChangedBy = userId
        });

        await db.SaveChangesAsync(cancellationToken);
    }
}
