namespace Internshala.Application.Common.Interfaces;

/// <summary>
/// Abstraction over SignalR hub to send real-time notifications to connected clients.
/// </summary>
public interface INotificationHubService
{
    Task SendNotificationAsync(int userId, string type, string title, string message, string? actionUrl, CancellationToken cancellationToken = default);
    Task SendUnreadCountAsync(int userId, int unreadCount, CancellationToken cancellationToken = default);
}
