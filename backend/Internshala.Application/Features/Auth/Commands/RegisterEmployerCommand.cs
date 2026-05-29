using Internshala.Application.Common.Interfaces;
using Internshala.Application.Features.Auth.DTOs;
using Internshala.Domain.Entities;
using Internshala.Domain.Enums;
using Internshala.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Internshala.Application.Features.Auth.Commands;

public sealed record RegisterEmployerCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string CompanyName,
    string? PhoneNumber) : IRequest<TokenResponseDto>;

public sealed class RegisterEmployerCommandHandler(
    IApplicationDbContext db,
    IJwtService jwtService,
    IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterEmployerCommand, TokenResponseDto>
{
    public async Task<TokenResponseDto> Handle(
        RegisterEmployerCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await db.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
        if (exists)
            throw new ConflictException($"An account with email '{request.Email}' already exists.");

        var defaultCategory = await db.Categories.FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Category", "default");

        var user = new User
        {
            Email = request.Email.ToLowerInvariant(),
            PasswordHash = passwordHasher.Hash(request.Password),
            Role = UserRole.Employer,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            IsActive = true
        };

        db.Users.Add(user);
        db.Employers.Add(new Employer
        {
            User = user,
            CompanyName = request.CompanyName,
            IndustryId = defaultCategory.Id
        });
        await db.SaveChangesAsync(cancellationToken);

        return RegisterStudentCommandHandler.BuildTokenResponse(user, jwtService, db);
    }
}
