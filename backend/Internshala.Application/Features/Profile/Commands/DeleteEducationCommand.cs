using Internshala.Application.Common.Interfaces;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Profile.Commands;

public sealed record DeleteEducationCommand(int EducationId) : IRequest;

public sealed class DeleteEducationCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<DeleteEducationCommand>
{
    public async Task Handle(DeleteEducationCommand command, CancellationToken cancellationToken)
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

        education.IsDeleted = true;
        await db.SaveChangesAsync(cancellationToken);
    }
}
