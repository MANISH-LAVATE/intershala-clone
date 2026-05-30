using Internshala.Application.Common.Models;
using Internshala.Application.Features.Applications.Commands;
using Internshala.Application.Features.Applications.DTOs;
using Internshala.Application.Features.Applications.Queries;
using Internshala.Shared.Constants;
using Internshala.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Internshala.API.Controllers.v1;

[ApiController]
[Route("api/v1/applications")]
[Produces("application/json")]
[Authorize]
public sealed class ApplicationsController(ISender mediator) : ControllerBase
{
    /// <summary>Apply to an internship or job listing.</summary>
    [Authorize(Roles = RoleConstants.Student)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Apply(
        [FromBody] ApplyRequest request,
        CancellationToken ct)
    {
        var id = await mediator.Send(new ApplyCommand(request), ct);
        return CreatedAtAction(nameof(GetApplication), new { id },
            ApiResponse<int>.Ok(id, "Application submitted successfully."));
    }

    /// <summary>Get the authenticated student's applications.</summary>
    [Authorize(Roles = RoleConstants.Student)]
    [HttpGet("my")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ApplicationListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyApplications(
        [FromQuery] ApplicationFilterParams filters,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetMyApplicationsQuery(filters), ct);
        return Ok(ApiResponse<ApplicationListDto>.Paginated(result));
    }

    /// <summary>Get a single application by ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<ApplicationDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetApplication(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetApplicationByIdQuery(id), ct);
        return Ok(ApiResponse<ApplicationDetailDto>.Ok(result));
    }

    /// <summary>List applications for a specific listing (employer only).</summary>
    [Authorize(Roles = RoleConstants.Employer + "," + RoleConstants.Admin)]
    [HttpGet("listing")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<EmployerApplicationListDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetListingApplications(
        [FromQuery] EmployerApplicationFilterParams filters,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetApplicationsForListingQuery(filters), ct);
        return Ok(ApiResponse<EmployerApplicationListDto>.Paginated(result));
    }

    /// <summary>Update an application's status (employer only).</summary>
    [Authorize(Roles = RoleConstants.Employer + "," + RoleConstants.Admin)]
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] UpdateApplicationStatusRequest request,
        CancellationToken ct)
    {
        await mediator.Send(new UpdateApplicationStatusCommand(id, request), ct);
        return NoContent();
    }

    /// <summary>Withdraw an application (student only).</summary>
    [Authorize(Roles = RoleConstants.Student)]
    [HttpPost("{id:int}/withdraw")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Withdraw(int id, CancellationToken ct)
    {
        await mediator.Send(new WithdrawApplicationCommand(id), ct);
        return NoContent();
    }
}
