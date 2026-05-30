using Internshala.Application.Common.Models;
using Internshala.Application.Features.Admin.Commands;
using Internshala.Application.Features.Admin.DTOs;
using Internshala.Application.Features.Admin.Queries;
using Internshala.Shared.Constants;
using Internshala.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Internshala.API.Controllers.v1;

[ApiController]
[Route("api/v1/admin")]
[Produces("application/json")]
[Authorize(Roles = RoleConstants.Admin)]
public sealed class AdminController(ISender mediator) : ControllerBase
{
    /// <summary>Get paginated user list with filtering.</summary>
    [HttpGet("users")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AdminUserDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(
        [FromQuery] AdminUserFilterParams filters,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetAdminUsersQuery(filters), ct);
        return Ok(ApiResponse<AdminUserDto>.Paginated(result));
    }

    /// <summary>Activate or deactivate a user account.</summary>
    [HttpPatch("users/{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserStatus(
        int id,
        [FromBody] UpdateUserStatusRequest request,
        CancellationToken ct)
    {
        await mediator.Send(new UpdateUserStatusCommand(id, request.IsActive), ct);
        return NoContent();
    }

    /// <summary>Get platform analytics summary.</summary>
    [HttpGet("analytics")]
    [ProducesResponseType(typeof(ApiResponse<AdminAnalyticsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAnalytics(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAdminAnalyticsQuery(), ct);
        return Ok(ApiResponse<AdminAnalyticsDto>.Ok(result));
    }
}

public sealed record UpdateUserStatusRequest(bool IsActive);
