using Internshala.Domain.Entities;

namespace Internshala.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    string HashToken(string token);
    int? GetUserIdFromToken(string token);
}
