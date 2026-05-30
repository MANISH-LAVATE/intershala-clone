using Internshala.Application.Common.Interfaces;
using Internshala.Shared.Constants;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Internships.Commands;

public sealed record DeleteInternshipCommand(int Id) : IRequest;

public sealed class DeleteInternshipCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<DeleteInternshipCommand>
{
    public async Task Handle(DeleteInternshipCommand command, CancellationToken cancellationToken)
    {
        var internship = await db.Internships
            .Include(i => i.Employer)
            .FirstOrDefaultAsync(i => i.Id == command.Id, cancellationToken)
            ?? throw new NotFoundException("Internship", command.Id);

        if (internship.Employer.UserId != currentUser.UserId && currentUser.Role != RoleConstants.Admin)
            throw new ForbiddenException("You can only delete your own internship listings.");

        internship.IsDeleted = true;
        internship.IsActive = false;
        await db.SaveChangesAsync(cancellationToken);
    }
}
