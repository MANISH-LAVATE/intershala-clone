using System.Security.Claims;
using Internshala.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Internshala.Infrastructure.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly ClaimsPrincipal? _user = httpContextAccessor.HttpContext?.User;

    public int? UserId
    {
        get
        {
            var value = _user?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? _user?.FindFirstValue("sub");
            return value != null && int.TryParse(value, out var id) ? id : null;
        }
    }

    public string? Email => _user?.FindFirstValue(ClaimTypes.Email)
        ?? _user?.FindFirstValue("email");

    public string? Role => _user?.FindFirstValue(ClaimTypes.Role);

    public bool IsAuthenticated => _user?.Identity?.IsAuthenticated ?? false;

    public string? IpAddress => httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

    public string? UserAgent => httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
}
