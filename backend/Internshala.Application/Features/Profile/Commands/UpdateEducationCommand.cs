using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Profile.DTOs;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Profile.Commands;

public sealed record UpdateEducationCommand(int EducationId, UpdateEducationRequest Request) : IRequest;

public sealed class UpdateEducationCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<UpdateEducationCommand>
{
    public async Task Handle(UpdateEducationCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? throw new UnauthorizedException();

        var student = await db.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken)
            ?? throw new NotFoundException("Student profile", userId);

        var education = await db.Educations
            .FirstOrDefaultAsync(e => e.Id == command.EducationId && e.StudentId == student.Id, cancellationToken)
            ?? throw new NotFoundException("Education", command.EducationId);

        var req = command.Request;
        education.Institution = req.Institution;
        education.Degree = req.Degree;
        education.FieldOfStudy = req.FieldOfStudy;
        education.StartYear = req.StartYear;
        education.EndYear = req.EndYear;
        education.IsCurrent = req.IsCurrent;
        education.Grade = req.Grade;

        await db.SaveChangesAsync(cancellationToken);
    }
}
