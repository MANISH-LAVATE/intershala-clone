using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Profile.DTOs;
using Internshala.Domain.Entities;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Profile.Commands;

public sealed record AddEducationCommand(AddEducationRequest Request) : IRequest<int>;

public sealed class AddEducationCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<AddEducationCommand, int>
{
    public async Task<int> Handle(AddEducationCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? throw new UnauthorizedException();

        var student = await db.Students
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken)
            ?? throw new NotFoundException("Student profile", userId);

        var req = command.Request;
        var education = new Education
        {
            StudentId = student.Id,
            Institution = req.Institution,
            Degree = req.Degree,
            FieldOfStudy = req.FieldOfStudy,
            StartYear = req.StartYear,
            EndYear = req.EndYear,
            IsCurrent = req.IsCurrent,
            Grade = req.Grade
        };

        db.Educations.Add(education);
        await db.SaveChangesAsync(cancellationToken);
        return education.Id;
    }
}
