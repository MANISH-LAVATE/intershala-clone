using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Auth.DTOs;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Auth.Commands;

public sealed record LoginCommand(string Email, string Password) : IRequest<TokenResponseDto>;

public sealed class LoginCommandHandler(
    IApplicationDbContext db,
    IJwtService jwtService,
    IPasswordHasher passwordHasher)
    : IRequestHandler<LoginCommand, TokenResponseDto>
{
    public async Task<TokenResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await db.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email.ToLowerInvariant(), cancellationToken)
            ?? throw new UnauthorizedException("Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedException("Your account has been deactivated.");

        if (user.IsLockedOut)
            throw new UnauthorizedException("Account is temporarily locked. Please try again later.");

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            user.FailedLoginCount++;
            if (user.FailedLoginCount >= 5)
                user.LockoutUntil = DateTime.UtcNow.AddMinutes(15);
            await db.SaveChangesAsync(cancellationToken);
            throw new UnauthorizedException("Invalid email or password.");
        }

        user.FailedLoginCount = 0;
        user.LockoutUntil = null;
        user.LastLoginAt = DateTime.UtcNow;

        var result = RegisterStudentCommandHandler.BuildTokenResponse(user, jwtService, db);
        await db.SaveChangesAsync(cancellationToken);

        return result;
    }
}
