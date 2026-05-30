using Internshala.Application.Common.Interfaces;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Admin.Commands;

public sealed record UpdateUserStatusCommand(int UserId, bool IsActive) : IRequest;

public sealed class UpdateUserStatusCommandHandler(IApplicationDbContext db)
    : IRequestHandler<UpdateUserStatusCommand>
{
    public async Task Handle(UpdateUserStatusCommand command, CancellationToken cancellationToken)
    {
        var user = await db.Users
            .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken)
            ?? throw new NotFoundException("User", command.UserId);

        user.IsActive = command.IsActive;
        await db.SaveChangesAsync(cancellationToken);
    }
}
