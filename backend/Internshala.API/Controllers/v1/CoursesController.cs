using Internshala.Application.Common.Models;
using Internshala.Application.Features.Courses.Commands;
using Internshala.Application.Features.Courses.DTOs;
using Internshala.Application.Features.Courses.Queries;
using Internshala.Shared.Constants;
using Internshala.Shared.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Internshala.API.Controllers.v1;

[ApiController]
[Route("api/v1/courses")]
[Produces("application/json")]
public sealed class CoursesController(ISender mediator) : ControllerBase
{
    /// <summary>List published courses with filtering.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CourseListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourses(
        [FromQuery] CourseFilterParams filters,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetCoursesQuery(filters), ct);
        return Ok(ApiResponse<CourseListDto>.Paginated(result));
    }

    /// <summary>Get a course by ID including modules.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<CourseDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourse(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetCourseByIdQuery(id), ct);
        return Ok(ApiResponse<CourseDetailDto>.Ok(result));
    }

    /// <summary>Enroll the authenticated student in a course.</summary>
    [Authorize(Roles = RoleConstants.Student)]
    [HttpPost("{id:int}/enroll")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Enroll(int id, CancellationToken ct)
    {
        await mediator.Send(new EnrollCommand(id), ct);
        return NoContent();
    }

    /// <summary>Get the authenticated student's enrollments.</summary>
    [Authorize(Roles = RoleConstants.Student)]
    [HttpGet("my")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<EnrollmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyEnrollments(CancellationToken ct)
    {
        var result = await mediator.Send(new GetMyEnrollmentsQuery(), ct);
        return Ok(ApiResponse<IReadOnlyList<EnrollmentDto>>.Ok(result));
    }
}
