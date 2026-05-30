using Internshala.Application.Common.Models;
using Internshala.Application.Features.Search.DTOs;
using Internshala.Application.Features.Search.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Internshala.API.Controllers.v1;

[ApiController]
[Route("api/v1/search")]
[Produces("application/json")]
public sealed class SearchController(ISender mediator) : ControllerBase
{
    /// <summary>Global search across internships, jobs, and courses.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GlobalSearchResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] string q,
        [FromQuery] int limit = 15,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GlobalSearchQuery(q ?? string.Empty, limit), ct);
        return Ok(ApiResponse<GlobalSearchResult>.Ok(result));
    }
}
