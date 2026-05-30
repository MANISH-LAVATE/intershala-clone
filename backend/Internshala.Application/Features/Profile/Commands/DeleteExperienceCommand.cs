using Internshala.Application.Common.Interfaces;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Profile.Commands;

public sealed record DeleteExperienceCommand(int ExperienceId) : IRequest;

public sealed class DeleteExperienceCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<DeleteExperienceCommand>
{
    public async Task Handle(DeleteExperienceCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? throw new UnauthorizedException();

        var student = await db.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken)
            ?? throw new NotFoundException("Student profile", userId);

        var experience = await db.Experiences
            .FirstOrDefaultAsync(e => e.Id == command.ExperienceId && e.StudentId == student.Id, cancellationToken)
            ?? throw new NotFoundException("Experience", command.ExperienceId);

        experience.IsDeleted = true;
        await db.SaveChangesAsync(cancellationToken);
    }
}
