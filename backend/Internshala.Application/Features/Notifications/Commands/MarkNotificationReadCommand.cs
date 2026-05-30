using Internshala.Application.Common.Interfaces;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Notifications.Commands;

public sealed record MarkNotificationReadCommand(int NotificationId) : IRequest;

public sealed class MarkNotificationReadCommandHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<MarkNotificationReadCommand>
{
    public async Task Handle(MarkNotificationReadCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedException();

        var notification = await db.Notifications
            .FirstOrDefaultAsync(n => n.Id == command.NotificationId && n.UserId == userId, cancellationToken)
            ?? throw new NotFoundException("Notification", command.NotificationId);

        if (!notification.IsRead)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
