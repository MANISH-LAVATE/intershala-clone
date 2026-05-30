using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Notifications.DTOs;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Notifications.Queries;

public sealed record GetNotificationsQuery(int Page = 1, int PageSize = 20) : IRequest<NotificationSummaryDto>;

public sealed class GetNotificationsQueryHandler(
    IApplicationDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<GetNotificationsQuery, NotificationSummaryDto>
{
    public async Task<NotificationSummaryDto> Handle(
        GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedException();

        var unreadCount = await db.Notifications
            .AsNoTracking()
            .CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);

        var items = await db.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(n => new NotificationDto(
                n.Id, n.Type, n.Title, n.Message,
                n.ActionUrl, n.IsRead, n.ReadAt, n.CreatedAt))
            .ToListAsync(cancellationToken);

        return new NotificationSummaryDto(items, unreadCount);
    }
}
