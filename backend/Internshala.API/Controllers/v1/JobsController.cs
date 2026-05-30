using Internshala.Application.Common.Models;
using Internshala.Application.Features.Jobs.Commands;
using Internshala.Application.Features.Jobs.DTOs;
using Internshala.Application.Features.Jobs.Queries;
using Internshala.Shared.Constants;
using Internshala.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Internshala.API.Controllers.v1;

[ApiController]
[Route("api/v1/jobs")]
[Produces("application/json")]
public sealed class JobsController(ISender mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<JobListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJobs([FromQuery] JobFilterParams filters, CancellationToken ct)
    {
        var result = await mediator.Send(new GetJobsQuery(filters), ct);
        return Ok(ApiResponse<JobListDto>.Paginated(result));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<JobDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJob(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetJobByIdQuery(id), ct);
        return Ok(ApiResponse<JobDetailDto>.Ok(result));
    }

    [Authorize(Roles = RoleConstants.Employer + "," + RoleConstants.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateJob(
        [FromBody] CreateJobRequest request,
        CancellationToken ct)
    {
        var id = await mediator.Send(new CreateJobCommand(request), ct);
        return CreatedAtAction(nameof(GetJob), new { id },
            ApiResponse<int>.Ok(id, "Job created successfully."));
    }
}
