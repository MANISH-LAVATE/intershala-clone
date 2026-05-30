namespace Internshala.Application.Features.Notifications.DTOs;

public sealed record NotificationDto(
    int Id,
    string Type,
    string Title,
    string Message,
    string? ActionUrl,
    bool IsRead,
    DateTime? ReadAt,
    DateTime CreatedAt);

public sealed record NotificationSummaryDto(
    IReadOnlyList<NotificationDto> Items,
    int UnreadCount);
