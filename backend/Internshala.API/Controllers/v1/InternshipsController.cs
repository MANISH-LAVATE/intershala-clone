using Internshala.Application.Common.Models;
using Internshala.Application.Features.Internships.Commands;
using Internshala.Application.Features.Internships.DTOs;
using Internshala.Application.Features.Internships.Queries;
using Internshala.Shared.Constants;
using Internshala.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Internshala.API.Controllers.v1;

[ApiController]
[Route("api/v1/internships")]
[Produces("application/json")]
public sealed class InternshipsController(ISender mediator) : ControllerBase
{
    [HttpGet]
    [OutputCache(PolicyName = "listings")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<InternshipListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInternships(
        [FromQuery] InternshipFilterParams filters,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetInternshipsQuery(filters), ct);
        return Ok(ApiResponse<InternshipListDto>.Paginated(result));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<InternshipDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInternship(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetInternshipByIdQuery(id), ct);
        return Ok(ApiResponse<InternshipDetailDto>.Ok(result));
    }

    [Authorize(Roles = RoleConstants.Employer + "," + RoleConstants.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateInternship(
        [FromBody] CreateInternshipRequest request,
        CancellationToken ct)
    {
        var id = await mediator.Send(new CreateInternshipCommand(request), ct);
        return CreatedAtAction(nameof(GetInternship), new { id },
            ApiResponse<int>.Ok(id, "Internship created successfully."));
    }

    [Authorize(Roles = RoleConstants.Employer + "," + RoleConstants.Admin)]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateInternship(
        int id,
        [FromBody] UpdateInternshipRequest request,
        CancellationToken ct)
    {
        await mediator.Send(new UpdateInternshipCommand(id, request), ct);
        return NoContent();
    }

    [Authorize(Roles = RoleConstants.Employer + "," + RoleConstants.Admin)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInternship(int id, CancellationToken ct)
    {
        await mediator.Send(new DeleteInternshipCommand(id), ct);
        return NoContent();
    }
}
