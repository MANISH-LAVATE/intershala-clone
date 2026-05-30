using FluentValidation;
using Internshala.Application.Features.Profile.DTOs;

namespace Internshala.Application.Features.Profile.Validators;

public sealed class UpdateStudentProfileRequestValidator : AbstractValidator<UpdateStudentProfileRequest>
{
    public UpdateStudentProfileRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.")
            .When(x => x.PhoneNumber is not null);

        RuleFor(x => x.Bio)
            .MaximumLength(1000).WithMessage("Bio must not exceed 1000 characters.")
            .When(x => x.Bio is not null);

        RuleFor(x => x.GraduationYear)
            .InclusiveBetween((short)1950, (short)2100)
            .WithMessage("Graduation year must be between 1950 and 2100.")
            .When(x => x.GraduationYear.HasValue);

        RuleFor(x => x.Gpa)
            .InclusiveBetween(0m, 10m)
            .WithMessage("GPA must be between 0 and 10.")
            .When(x => x.Gpa.HasValue);

        RuleFor(x => x.LinkedInUrl)
            .MaximumLength(500)
            .When(x => x.LinkedInUrl is not null);

        RuleFor(x => x.GitHubUrl)
            .MaximumLength(500)
            .When(x => x.GitHubUrl is not null);

        RuleFor(x => x.PortfolioUrl)
            .MaximumLength(500)
            .When(x => x.PortfolioUrl is not null);
    }
}

public sealed class AddEducationRequestValidator : AbstractValidator<AddEducationRequest>
{
    public AddEducationRequestValidator()
    {
        RuleFor(x => x.Institution).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Degree).NotEmpty().MaximumLength(200);
        RuleFor(x => x.FieldOfStudy).NotEmpty().MaximumLength(200);
        RuleFor(x => x.StartYear)
            .InclusiveBetween((short)1950, (short)2100)
            .WithMessage("Start year must be between 1950 and 2100.");
        RuleFor(x => x.EndYear)
            .GreaterThanOrEqualTo(x => x.StartYear)
            .WithMessage("End year must be after start year.")
            .When(x => x.EndYear.HasValue);
        RuleFor(x => x.Grade)
            .InclusiveBetween(0m, 100m)
            .When(x => x.Grade.HasValue);
    }
}

public sealed class AddExperienceRequestValidator : AbstractValidator<AddExperienceRequest>
{
    public AddExperienceRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Company).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(3000).When(x => x.Description is not null);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date.")
            .When(x => x.EndDate.HasValue);
    }
}
