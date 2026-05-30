using Internshala.Application.Common.Models;
using Internshala.Application.Features.Profile.Commands;
using Internshala.Application.Features.Profile.DTOs;
using Internshala.Application.Features.Profile.Queries;
using Internshala.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Internshala.API.Controllers.v1;

[ApiController]
[Route("api/v1/profile")]
[Produces("application/json")]
[Authorize(Roles = RoleConstants.Student)]
public sealed class ProfileController(ISender mediator) : ControllerBase
{
    /// <summary>Get the authenticated student's full profile.</summary>
    [HttpGet("student")]
    [ProducesResponseType(typeof(ApiResponse<StudentProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile(CancellationToken ct)
    {
        var result = await mediator.Send(new GetStudentProfileQuery(), ct);
        return Ok(ApiResponse<StudentProfileDto>.Ok(result));
    }

    /// <summary>Update the authenticated student's profile.</summary>
    [HttpPut("student")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateStudentProfileRequest request,
        CancellationToken ct)
    {
        await mediator.Send(new UpdateStudentProfileCommand(request), ct);
        return NoContent();
    }

    /// <summary>Add an education entry.</summary>
    [HttpPost("education")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddEducation(
        [FromBody] AddEducationRequest request,
        CancellationToken ct)
    {
        var id = await mediator.Send(new AddEducationCommand(request), ct);
        return CreatedAtAction(nameof(GetProfile), null,
            ApiResponse<int>.Ok(id, "Education added successfully."));
    }

    /// <summary>Update an education entry.</summary>
    [HttpPut("education/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEducation(
        int id,
        [FromBody] UpdateEducationRequest request,
        CancellationToken ct)
    {
        await mediator.Send(new UpdateEducationCommand(id, request), ct);
        return NoContent();
    }

    /// <summary>Delete an education entry.</summary>
    [HttpDelete("education/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEducation(int id, CancellationToken ct)
    {
        await mediator.Send(new DeleteEducationCommand(id), ct);
        return NoContent();
    }

    /// <summary>Add an experience entry.</summary>
    [HttpPost("experience")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddExperience(
        [FromBody] AddExperienceRequest request,
        CancellationToken ct)
    {
        var id = await mediator.Send(new AddExperienceCommand(request), ct);
        return CreatedAtAction(nameof(GetProfile), null,
            ApiResponse<int>.Ok(id, "Experience added successfully."));
    }

    /// <summary>Update an experience entry.</summary>
    [HttpPut("experience/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateExperience(
        int id,
        [FromBody] UpdateExperienceRequest request,
        CancellationToken ct)
    {
        await mediator.Send(new UpdateExperienceCommand(id, request), ct);
        return NoContent();
    }

    /// <summary>Delete an experience entry.</summary>
    [HttpDelete("experience/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteExperience(int id, CancellationToken ct)
    {
        await mediator.Send(new DeleteExperienceCommand(id), ct);
        return NoContent();
    }
}
