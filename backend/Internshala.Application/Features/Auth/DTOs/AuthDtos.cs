using Internshala.Domain.Enums;

namespace Internshala.Application.Features.Auth.DTOs;

public sealed record AuthUserDto(
    int Id,
    string Email,
    string FirstName,
    string LastName,
    UserRole Role,
    string? ProfilePictureUrl,
    bool IsEmailVerified);

public sealed record TokenResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    AuthUserDto User);

public sealed record RegisterStudentRequest(
    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    string? PhoneNumber);

public sealed record RegisterEmployerRequest(
    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    string CompanyName,
    string? PhoneNumber);

public sealed record LoginRequest(
    string Email,
    string Password);

public sealed record RefreshTokenRequest(
    string RefreshToken);

public sealed record ForgotPasswordRequest(
    string Email);

public sealed record ResetPasswordRequest(
    string Token,
    string Password,
    string ConfirmPassword);
