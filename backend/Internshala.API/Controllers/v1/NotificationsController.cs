using Internshala.Application.Common.Models;
using Internshala.Application.Features.Notifications.Commands;
using Internshala.Application.Features.Notifications.DTOs;
using Internshala.Application.Features.Notifications.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Internshala.API.Controllers.v1;

[ApiController]
[Route("api/v1/notifications")]
[Produces("application/json")]
[Authorize]
public sealed class NotificationsController(ISender mediator) : ControllerBase
{
    /// <summary>Get the authenticated user's notifications.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<NotificationSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotifications(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetNotificationsQuery(page, pageSize), ct);
        return Ok(ApiResponse<NotificationSummaryDto>.Ok(result));
    }

    /// <summary>Mark a single notification as read.</summary>
    [HttpPost("{id:int}/read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkRead(int id, CancellationToken ct)
    {
        await mediator.Send(new MarkNotificationReadCommand(id), ct);
        return NoContent();
    }

    /// <summary>Mark all notifications as read.</summary>
    [HttpPost("read-all")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> MarkAllRead(CancellationToken ct)
    {
        await mediator.Send(new MarkAllNotificationsReadCommand(), ct);
        return NoContent();
    }
}
