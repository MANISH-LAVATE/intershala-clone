using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Profile.DTOs;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Profile.Commands;

public sealed record UpdateExperienceCommand(int ExperienceId, UpdateExperienceRequest Request) : IRequest;

public sealed class UpdateExperienceCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<UpdateExperienceCommand>
{
    public async Task Handle(UpdateExperienceCommand command, CancellationToken cancellationToken)
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

        var req = command.Request;
        experience.Title = req.Title;
        experience.Company = req.Company;
        experience.Description = req.Description;
        experience.StartDate = req.StartDate;
        experience.EndDate = req.EndDate;
        experience.IsCurrent = req.IsCurrent;

        await db.SaveChangesAsync(cancellationToken);
    }
}
