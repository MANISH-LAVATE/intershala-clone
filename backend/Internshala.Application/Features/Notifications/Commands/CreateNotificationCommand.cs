using Internshala.Application.Common.Interfaces;
using Internshala.Domain.Entities;
using MediatR;

namespace Internshala.Application.Features.Notifications.Commands;

public sealed record CreateNotificationCommand(
    int UserId,
    string Type,
    string Title,
    string Message,
    string? ActionUrl = null) : IRequest;

public sealed class CreateNotificationCommandHandler(IApplicationDbContext db)
    : IRequestHandler<CreateNotificationCommand>
{
    public async Task Handle(CreateNotificationCommand command, CancellationToken cancellationToken)
    {
        db.Notifications.Add(new Notification
        {
            UserId = command.UserId,
            Type = command.Type,
            Title = command.Title,
            Message = command.Message,
            ActionUrl = command.ActionUrl,
            IsRead = false
        });

        await db.SaveChangesAsync(cancellationToken);
    }
}
