using Internshala.Application.Common.Interfaces;
using Internshala.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Internshala.Infrastructure.Services;

public sealed class NotificationHubService(IHubContext<NotificationHub> hubContext)
    : INotificationHubService
{
    public async Task SendNotificationAsync(
        int userId, string type, string title, string message,
        string? actionUrl, CancellationToken cancellationToken = default)
    {
        await hubContext.Clients
            .Group($"user-{userId}")
            .SendAsync("ReceiveNotification", new
            {
                type,
                title,
                message,
                actionUrl,
                createdAt = DateTime.UtcNow
            }, cancellationToken);
    }

    public async Task SendUnreadCountAsync(int userId, int unreadCount, CancellationToken cancellationToken = default)
    {
        await hubContext.Clients
            .Group($"user-{userId}")
            .SendAsync("UnreadCount", unreadCount, cancellationToken);
    }
}
