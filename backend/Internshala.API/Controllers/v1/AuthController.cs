using Internshala.Application.Common.Models;
using Internshala.Application.Features.Auth.Commands;
using Internshala.Application.Features.Auth.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Internshala.API.Controllers.v1;

[ApiController]
[Route("api/v1/auth")]
[Produces("application/json")]
public sealed class AuthController(ISender mediator) : ControllerBase
{
    [EnableRateLimiting("auth")]
    [HttpPost("register/student")]
    [ProducesResponseType(typeof(ApiResponse<TokenResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RegisterStudent(
        [FromBody] RegisterStudentRequest request,
        CancellationToken ct)
    {
        var result = await mediator.Send(new RegisterStudentCommand(
            request.Email, request.Password, request.FirstName, request.LastName, request.PhoneNumber), ct);

        return StatusCode(StatusCodes.Status201Created, ApiResponse<TokenResponseDto>.Ok(result, "Registration successful."));
    }

    [EnableRateLimiting("auth")]
    [HttpPost("register/employer")]
    [ProducesResponseType(typeof(ApiResponse<TokenResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterEmployer(
        [FromBody] RegisterEmployerRequest request,
        CancellationToken ct)
    {
        var result = await mediator.Send(new RegisterEmployerCommand(
            request.Email, request.Password, request.FirstName, request.LastName,
            request.CompanyName, request.PhoneNumber), ct);

        return StatusCode(StatusCodes.Status201Created, ApiResponse<TokenResponseDto>.Ok(result, "Registration successful."));
    }

    [EnableRateLimiting("auth")]
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<TokenResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new LoginCommand(request.Email, request.Password), ct);
        return Ok(ApiResponse<TokenResponseDto>.Ok(result));
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse<TokenResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new RefreshTokenCommand(request.RefreshToken), ct);
        return Ok(ApiResponse<TokenResponseDto>.Ok(result));
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {
        await mediator.Send(new LogoutCommand(request.RefreshToken), ct);
        return NoContent();
    }
}
