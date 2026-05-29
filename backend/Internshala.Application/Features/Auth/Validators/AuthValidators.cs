using FluentValidation;
using Internshala.Application.Features.Auth.Commands;

namespace Internshala.Application.Features.Auth.Validators;

public sealed class RegisterStudentCommandValidator : AbstractValidator<RegisterStudentCommand>
{
    private const string PasswordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

    public RegisterStudentCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().Matches(PasswordPattern)
            .WithMessage("Password must be at least 8 characters with 1 uppercase, 1 number, and 1 special character.");
        RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PhoneNumber).MaximumLength(15).When(x => x.PhoneNumber != null);
    }
}

public sealed class RegisterEmployerCommandValidator : AbstractValidator<RegisterEmployerCommand>
{
    private const string PasswordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

    public RegisterEmployerCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().Matches(PasswordPattern)
            .WithMessage("Password must be at least 8 characters with 1 uppercase, 1 number, and 1 special character.");
        RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CompanyName).NotEmpty().MinimumLength(2).MaximumLength(200);
    }
}

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
