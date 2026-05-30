using Internshala.Application.Common.Models;
using Internshala.Application.Features.Lookup;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Internshala.API.Controllers.v1;

[ApiController]
[Route("api/v1/lookups")]
[Produces("application/json")]
public sealed class LookupsController(ISender mediator, IMemoryCache cache) : ControllerBase
{
    [HttpGet("categories")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CategoryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories(CancellationToken ct)
    {
        var result = await cache.GetOrCreateAsync("categories:all", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            return await mediator.Send(new GetCategoriesQuery(), ct);
        });

        return Ok(ApiResponse<IReadOnlyList<CategoryDto>>.Ok(result!));
    }

    [HttpGet("locations")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<LocationDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocations([FromQuery] string? search, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLocationsQuery(search), ct);
        return Ok(ApiResponse<IReadOnlyList<LocationDto>>.Ok(result));
    }

    [HttpGet("skills")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SkillDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSkills([FromQuery] string? search, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSkillsQuery(search), ct);
        return Ok(ApiResponse<IReadOnlyList<SkillDto>>.Ok(result));
    }
}
