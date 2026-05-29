using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Auth.DTOs;
using Internshala.Domain.Entities;
using Internshala.Domain.Enums;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Auth.Commands;

public sealed record RegisterStudentCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? PhoneNumber) : IRequest<TokenResponseDto>;

public sealed class RegisterStudentCommandHandler(
    IApplicationDbContext db,
    IJwtService jwtService,
    IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterStudentCommand, TokenResponseDto>
{
    public async Task<TokenResponseDto> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
    {
        var exists = await db.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
        if (exists)
            throw new ConflictException($"An account with email '{request.Email}' already exists.");

        var user = new User
        {
            Email = request.Email.ToLowerInvariant(),
            PasswordHash = passwordHasher.Hash(request.Password),
            Role = UserRole.Student,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            IsActive = true
        };

        db.Users.Add(user);
        db.Students.Add(new Student { User = user });
        await db.SaveChangesAsync(cancellationToken);

        return BuildTokenResponse(user, jwtService, db);
    }

    internal static TokenResponseDto BuildTokenResponse(User user, IJwtService jwtService, IApplicationDbContext db)
    {
        var accessToken = jwtService.GenerateAccessToken(user);
        var rawRefreshToken = jwtService.GenerateRefreshToken();

        db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = jwtService.HashToken(rawRefreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });

        return new TokenResponseDto(
            accessToken,
            rawRefreshToken,
            DateTime.UtcNow.AddMinutes(15),
            new AuthUserDto(user.Id, user.Email, user.FirstName, user.LastName,
                user.Role, user.ProfilePictureUrl, user.IsEmailVerified));
    }
}
