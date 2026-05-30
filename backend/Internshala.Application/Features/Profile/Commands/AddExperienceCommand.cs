using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Profile.DTOs;
using Internshala.Domain.Entities;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Profile.Commands;

public sealed record AddExperienceCommand(AddExperienceRequest Request) : IRequest<int>;

public sealed class AddExperienceCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<AddExperienceCommand, int>
{
    public async Task<int> Handle(AddExperienceCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? throw new UnauthorizedException();

        var student = await db.Students
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken)
            ?? throw new NotFoundException("Student profile", userId);

        var req = command.Request;
        var experience = new Experience
        {
            StudentId = student.Id,
            Title = req.Title,
            Company = req.Company,
            Description = req.Description,
            StartDate = req.StartDate,
            EndDate = req.EndDate,
            IsCurrent = req.IsCurrent
        };

        db.Experiences.Add(experience);
        await db.SaveChangesAsync(cancellationToken);
        return experience.Id;
    }
}
