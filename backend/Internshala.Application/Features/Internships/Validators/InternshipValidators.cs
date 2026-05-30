using FluentValidation;
using Internshala.Application.Features.Internships.Commands;

namespace Internshala.Application.Features.Internships.Validators;

public sealed class CreateInternshipCommandValidator : AbstractValidator<CreateInternshipCommand>
{
    private static readonly string[] ValidTypes = ["InOffice", "Remote", "Hybrid"];
    private static readonly string[] ValidStartTypes = ["Immediate", "Specific", "Flexible"];

    public CreateInternshipCommandValidator()
    {
        RuleFor(x => x.Request.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Request.Description).NotEmpty().MinimumLength(50);
        RuleFor(x => x.Request.CategoryId).GreaterThan(0);
        RuleFor(x => x.Request.InternshipType).NotEmpty().Must(t => ValidTypes.Contains(t))
            .WithMessage("InternshipType must be InOffice, Remote, or Hybrid.");
        RuleFor(x => x.Request.DurationMonths).InclusiveBetween((byte)1, (byte)24);
        RuleFor(x => x.Request.OpeningsCount).GreaterThan((short)0).LessThanOrEqualTo((short)500);
        RuleFor(x => x.Request.StartDateType).Must(t => ValidStartTypes.Contains(t))
            .WithMessage("StartDateType must be Immediate, Specific, or Flexible.");
        RuleFor(x => x.Request.StipendMin).GreaterThanOrEqualTo(0).When(x => x.Request.StipendMin.HasValue);
        RuleFor(x => x.Request.StipendMax)
            .GreaterThanOrEqualTo(x => x.Request.StipendMin)
            .When(x => x.Request.StipendMin.HasValue && x.Request.StipendMax.HasValue)
            .WithMessage("StipendMax must be >= StipendMin.");
    }
}

public sealed class UpdateInternshipCommandValidator : AbstractValidator<UpdateInternshipCommand>
{
    private static readonly string[] ValidTypes = ["InOffice", "Remote", "Hybrid"];

    public UpdateInternshipCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Request.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Request.Description).NotEmpty().MinimumLength(50);
        RuleFor(x => x.Request.CategoryId).GreaterThan(0);
        RuleFor(x => x.Request.InternshipType).NotEmpty().Must(t => ValidTypes.Contains(t));
        RuleFor(x => x.Request.DurationMonths).InclusiveBetween((byte)1, (byte)24);
        RuleFor(x => x.Request.OpeningsCount).GreaterThan((short)0);
    }
}
