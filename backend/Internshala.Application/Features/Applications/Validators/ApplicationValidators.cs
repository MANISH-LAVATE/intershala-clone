using FluentValidation;
using Internshala.Application.Features.Applications.DTOs;
using Internshala.Domain.Enums;

namespace Internshala.Application.Features.Applications.Validators;

public sealed class ApplyRequestValidator : AbstractValidator<ApplyRequest>
{
    public ApplyRequestValidator()
    {
        RuleFor(x => x.ListingType)
            .NotEmpty()
            .Must(t => t == "Internship" || t == "Job")
            .WithMessage("ListingType must be 'Internship' or 'Job'.");

        RuleFor(x => x.ListingId)
            .GreaterThan(0).WithMessage("ListingId must be a positive integer.");

        RuleFor(x => x.ResumeUrl)
            .NotEmpty().WithMessage("Resume URL is required.")
            .MaximumLength(1000).WithMessage("Resume URL must not exceed 1000 characters.");

        RuleFor(x => x.CoverLetter)
            .MaximumLength(5000).WithMessage("Cover letter must not exceed 5000 characters.")
            .When(x => x.CoverLetter is not null);

        RuleFor(x => x.ExpectedStipend)
            .GreaterThanOrEqualTo(0).WithMessage("Expected stipend cannot be negative.")
            .When(x => x.ExpectedStipend.HasValue);
    }
}

public sealed class UpdateApplicationStatusRequestValidator : AbstractValidator<UpdateApplicationStatusRequest>
{
    public UpdateApplicationStatusRequestValidator()
    {
        RuleFor(x => x.NewStatus)
            .IsInEnum().WithMessage("Invalid application status.");

        RuleFor(x => x.Note)
            .MaximumLength(1000).WithMessage("Note must not exceed 1000 characters.")
            .When(x => x.Note is not null);
    }
}
