using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Auth.DTOs;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Auth.Commands;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<TokenResponseDto>;

public sealed class RefreshTokenCommandHandler(
    IApplicationDbContext db,
    IJwtService jwtService)
    : IRequestHandler<RefreshTokenCommand, TokenResponseDto>
{
    public async Task<TokenResponseDto> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var tokenHash = jwtService.HashToken(request.RefreshToken);

        var storedToken = await db.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken)
            ?? throw new UnauthorizedException("Invalid or expired refresh token.");

        if (!storedToken.IsActive)
            throw new UnauthorizedException("Refresh token has been revoked or expired.");

        storedToken.RevokedAt = DateTime.UtcNow;

        var result = RegisterStudentCommandHandler.BuildTokenResponse(storedToken.User, jwtService, db);
        storedToken.ReplacedByToken = jwtService.HashToken(result.RefreshToken);

        await db.SaveChangesAsync(cancellationToken);

        return result;
    }
}
