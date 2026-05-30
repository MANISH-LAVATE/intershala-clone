using Internshala.Application.Common.Interfaces;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Notifications.Commands;

public sealed record MarkAllNotificationsReadCommand : IRequest;

public sealed class MarkAllNotificationsReadCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<MarkAllNotificationsReadCommand>
{
    public async Task Handle(MarkAllNotificationsReadCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedException();

        await db.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(s => s
                .SetProperty(n => n.IsRead, true)
                .SetProperty(n => n.ReadAt, DateTime.UtcNow),
            cancellationToken);
    }
}
